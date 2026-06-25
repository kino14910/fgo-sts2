using BaseLib.Abstracts;
using BaseLib.Utils;
using Fgo.FgoCode.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using System.Reflection;
using Fgo.FgoCode.Utils;

namespace Fgo.FgoCode.Character;

public class NobleCardPool : CustomCardPoolModel
{
    public override string Title => "Noble"; //This is not a display name.

    public override string BigEnergyIconPath => "charui/big_energy_noble.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    
    /* These HSV values will determine the color of your card back.
    They are applied as an shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 1f; //Hue; changes the color. (cc8f2b gold)
    public override float S => 1f; //Saturation
    public override float V => 1f; //Brightness

    private static Texture2D? _transparent;
    internal static Texture2D TransparentTexture =>
        _transparent ??= ImageTexture.CreateFromImage(Image.CreateEmpty(1, 1, false, Image.Format.Rgba8));

    //去掉卡框：返回透明纹理
    public override Texture2D CustomFrame(CustomCardModel card) => TransparentTexture;

    //Color of small card icons
    public override Color DeckEntryCardColor => new("cc8f2b");

    public override bool IsColorless => false;
    public override bool IsShared => true; //注册到 AllSharedCardPools，使卡牌出现在总览中
}

/// <summary>
/// 将 NoblePhantasm 稀有度映射为 Ancient，复用游戏内置的 Ancient 全画幅渲染管线。
/// NCard.Reload() 中已包含完整的 Ancient 全画幅逻辑：
///   - 全卡尺寸卡图 (_ancientPortrait)
///   - 全卡边框 (_ancientBorder)
///   - 文字底板 (_ancientTextBg，按卡类型自动选择)
///   - CanvasGroup 遮罩裁剪
///   - 发光 overlay (_ancientHighlight)
/// 此补丁通过 Prefix 修改 Rarity getter 返回值，使上述逻辑自动生效。
/// 注意：使用 backing field 判断以避免调用 getter 导致无限递归。
/// 横幅已通过 NobleHideBannerPatch 强制隐藏。
/// </summary>
[HarmonyPatch(typeof(CardModel), nameof(CardModel.Rarity), MethodType.Getter)]
static class NoblePhantasmRarityPatch
{
    //缓存 backing field（编译器为 auto-property 生成的 <Rarity>k__BackingField）
    private static readonly FieldInfo RarityField =
        AccessTools.Field(typeof(CardModel), "<Rarity>k__BackingField")!;

    [HarmonyPrefix]
    static bool MakeAncient(CardModel __instance, ref CardRarity __result)
    {
        var actual = (CardRarity)RarityField.GetValue(__instance)!;
        if (__instance.Pool is NobleCardPool && actual == FgoEnums.NoblePhantasm)
        {
            __result = CardRarity.Ancient;
            return false; //跳过原方法，使用修改后的值
        }
        return true;
    }
}

/// <summary>
/// 修复：自定义稀有度 NoblePhantasm 不被 GetCardRarityComparisonValue 识别，导致排序崩溃。
/// 注意：由于 NoblePhantasmRarityPatch 将 Rarity 映射为 Ancient，
/// 此补丁现在主要作为安全回退，正常情况下不会触发。
/// </summary>
[HarmonyPatch(typeof(NCardGrid), "GetCardRarityComparisonValue")]
static class NobleRaritySortPatch
{
    [HarmonyPrefix]
    static bool HandleNoblePhantasmRarity(CardModel a, ref int __result)
    {
        if (a.Pool is NobleCardPool && a.Rarity == FgoEnums.NoblePhantasm)
        {
            __result = 11; //排在 Token(10) 之后
            return false; //跳过原方法
        }
        return true; //其他稀有度走原方法
    }
}

/// <summary>
/// 在卡牌总览中为 NobleCardPool 添加筛选按钮。
/// BaseLib 只为 CustomCharacterModel 自动生成按钮，NobleCardPool 是共享池，需手动添加。
/// </summary>
[HarmonyPatch(typeof(NCardLibrary), nameof(NCardLibrary._Ready))]
[HarmonyPriority(Priority.High)]
static class NoblePoolFilterPatch
{
    public static NCardPoolFilter? NobleFilter;

    [HarmonyPostfix]
    static void AddNobleFilter(NCardLibrary __instance,
        Dictionary<NCardPoolFilter, Func<CardModel, bool>> ____poolFilters,
        Dictionary<NCardRarityTickbox, Func<CardModel, bool>> ____rarityFilters)
    {
        if (____poolFilters.Count == 0) return;
        if (____poolFilters.Last().Key.GetParentControl() is not GridContainer parent) return;

        NCardPoolFilter filter = CreateFilter();
        parent.AddChild(filter);
        NobleFilter = filter;

        ____poolFilters.Add(filter, c => c.Pool is NobleCardPool);

        //NobleCardPool 的卡总是通过稀有度筛选（模仿 misc/ancients 的行为）
        var keys = ____rarityFilters.Keys.ToList();
        foreach (var key in keys)
        {
            var originalFunc = ____rarityFilters[key];
            ____rarityFilters[key] = c => c.Pool is NobleCardPool || originalFunc(c);
        }

        MethodInfo updateMethod = AccessTools.Method(typeof(NCardLibrary), "UpdateCardPoolFilter")!;
        filter.Connect(NCardPoolFilter.SignalName.Toggled,
            Callable.From<NCardPoolFilter>(f => updateMethod.Invoke(__instance, [f])));

        FieldInfo lastHoveredField = AccessTools.DeclaredField(typeof(NCardLibrary), "_lastHoveredControl");
        if (lastHoveredField != null)
        {
            filter.Connect(Control.SignalName.FocusEntered,
                Callable.From(() => lastHoveredField.SetValue(__instance, filter)));
        }
    }

    static NCardPoolFilter CreateFilter()
    {
        NCardPoolFilter filter = new()
        {
            Name = "FILTER-Noble",
            Size = new(64, 64),
            CustomMinimumSize = new(64, 64),
            FocusMode = Control.FocusModeEnum.All
        };

        Texture2D tex = PreloadManager.Cache.GetTexture2D("character_icon_char_name.png".CharacterUiPath());

        TextureRect image = new()
        {
            Name = "Image",
            Texture = tex,
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
            Size = new(56, 56),
            Position = new(4, 4),
            Scale = new(0.9f, 0.9f),
            PivotOffset = new(28, 28),
            Material = ShaderUtils.GenerateHsv(1, 1, 1)
        };

        TextureRect shadow = new()
        {
            Name = "Shadow",
            Texture = tex,
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
            Size = new(56, 56),
            Position = new(4, 3),
            PivotOffset = new(28, 28),
            ShowBehindParent = true,
            Modulate = Colors.Black with { A = 0.25f },
        };

        image.AddChild(shadow);

        NSelectionReticle reticle = PreloadManager.Cache
            .GetScene(SceneHelper.GetScenePath("ui/selection_reticle"))
            .Instantiate<NSelectionReticle>();
        reticle.Name = "SelectionReticle";
        reticle.UniqueNameInOwner = true;

        filter.AddChild(image);
        image.Owner = filter;
        filter.AddChild(reticle);
        reticle.Owner = filter;

        return filter;
    }
}

/// <summary>
/// 使基底方法将 Noble 池视为特殊池（与 misc/ancients 相同），从而正确禁用稀有度筛选。
/// Noble 被选中时：禁用 tickbox → 调用 UpdateFilter → 跳过基底方法。
/// </summary>
[HarmonyPatch(typeof(NCardLibrary), "UpdateCardPoolFilter")]
static class NobleRarityPatch
{
    private static MethodInfo? _updateFilterMethod;

    [HarmonyPrefix]
    [HarmonyPriority(Priority.Low)]
    static bool HandleNobleRarity(
        NCardLibrary __instance,
        NCardPoolFilter ____miscPoolFilter,
        NCardPoolFilter ____ancientsFilter,
        Dictionary<NCardPoolFilter, Func<CardModel, bool>> ____poolFilters,
        Dictionary<NCardRarityTickbox, Func<CardModel, bool>> ____rarityFilters)
    {
        var noble = NoblePoolFilterPatch.NobleFilter;
        if (noble == null || !noble.IsSelected) return true;

        //radio-button 逻辑（取消选择其他池）
        foreach (var key in ____poolFilters.Keys)
            if (key != noble) key.IsSelected = false;

        //禁用所有稀有度 tickbox（与 misc/ancients 行为一致）
        foreach (var tickbox in ____rarityFilters.Keys)
            tickbox.Disable();

        //调用 UpdateFilter 刷新卡牌显示（基底方法被跳过，需手动调用）
        _updateFilterMethod ??= AccessTools.Method(typeof(NCardLibrary), "UpdateFilter", [typeof(bool)]);
        _updateFilterMethod.Invoke(__instance, [false]);

        return false; //跳过基底方法（radio-button 已处理，tickbox 已禁用，UpdateFilter 已调用）
    }

    //当卡牌总览关闭时，清空静态引用，防止生命周期残留
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NCardLibrary), nameof(NCardLibrary.OnSubmenuClosed))]
    static void ClearNobleFilterOnClose()
    {
        NoblePoolFilterPatch.NobleFilter = null;
    }
}

/// <summary>
/// 在 NCard.Reload() 后强制隐藏 NobleCardPool 卡牌的横幅。
/// NCard.Reload() 会显示 _ancientBanner，此补丁将其隐藏。
/// </summary>
[HarmonyPatch(typeof(NCard), "Reload")]
static class NobleHideBannerPatch
{
    // 缓存字段信息
    private static readonly FieldInfo AncientBannerField =
        AccessTools.Field(typeof(NCard), "_ancientBanner")!;
    private static readonly FieldInfo BannerField =
        AccessTools.Field(typeof(NCard), "_banner")!;
    private static readonly FieldInfo ModelField =
        AccessTools.Field(typeof(NCard), "_model")!;

    [HarmonyPostfix]
    static void HideBanner(NCard __instance)
    {
        var model = (CardModel?)ModelField.GetValue(__instance);
        if (model?.Pool is not NobleCardPool) return;

        // 强制隐藏横幅
        var ancientBanner = (Control?)AncientBannerField.GetValue(__instance);
        var banner = (TextureRect?)BannerField.GetValue(__instance);

        if (ancientBanner != null)
            ancientBanner.Visible = false;

        if (banner != null)
            banner.Visible = false;
    }
}
