using BaseLib.Abstracts;
using BaseLib.Utils;
using BaseLib.Utils.Patching;
using Fgo.FgoCode.Extensions;
using Fgo.FgoCode.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using System.Reflection;
using System.Reflection.Emit;

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

    //去掉卡框：返回透明纹理（卡背/牌库缩略图使用）
    public override Texture2D CustomFrame(CustomCardModel card) => TransparentTexture;

    //Color of small card icons
    public override Color DeckEntryCardColor => new("cc8f2b");

    public override bool IsColorless => false;
    public override bool IsShared => true; //注册到 AllSharedCardPools，使卡牌出现在总览中
}

//NoblePhantasm 全画幅：复用 Ancient 的全卡渲染管线
//Postfix 在 NCard.Reload() 之后执行，将 NoblePhantasm 卡的可见性切换为 Ancient 模式
[HarmonyPatch(typeof(NCard), "Reload")]
static class NobleFullArtPatch
{
    [HarmonyPostfix]
    static void ApplyNobleFullArt(
        NCard __instance,
        TextureRect ____ancientPortrait,
        TextureRect ____portrait,
        TextureRect ____frame,
        TextureRect ____portraitBorder,
        TextureRect ____banner,
        Control ____ancientBanner,
        TextureRect ____ancientBorder,
        TextureRect ____ancientTextBg,
        CanvasGroup ____portraitCanvasGroup,
        Material? ____canvasGroupMaskMaterial,
        Material? ____canvasGroupMaskBlurMaterial)
    {
        var model = __instance.Model;
        if (model == null || model.Rarity != FgoEnums.NoblePhantasm) return;

        //--- 可见性：和 Ancient 相同的全画幅模式 ---
        ____portraitBorder.Visible = false;
        ____portrait.Visible = false;
        ____frame.Visible = false;
        ____ancientPortrait.Visible = true;
        ____ancientBorder.Visible = true;
        ____ancientTextBg.Visible = true;

        //--- Noble 特有：不要横幅（AncientBanner 是火焰动画横幅）---
        ____banner.Visible = false;
        ____ancientBanner.Visible = false;

        //--- 卡图：基础方法已将 Portrait 赋给 _portrait，移到 _ancientPortrait ---
        ____ancientPortrait.Texture = ____portrait.Texture;

        //--- 遮罩：裁剪全卡尺寸的卡图到卡牌轮廓 ---
        if (____canvasGroupMaskMaterial != null)
            ____portraitCanvasGroup.Material = ____canvasGroupMaskMaterial;
        else if (____canvasGroupMaskBlurMaterial != null)
            ____portraitCanvasGroup.Material = ____canvasGroupMaskBlurMaterial;

        ____portrait.Material = null;
        ____ancientPortrait.Material = null;
    }
}

//修复：自定义稀有度 NoblePhantasm 不被 NCardGrid.GetCardRarityComparisonValue 识别，导致排序崩溃
[HarmonyPatch(typeof(NCardGrid), "GetCardRarityComparisonValue")]
static class NobleRaritySortPatch
{
    [HarmonyPrefix]
    static bool HandleNoblePhantasmRarity(CardModel a, ref int __result)
    {
        if (a.Rarity == FgoEnums.NoblePhantasm)
        {
            __result = 11; //排在 Token(10) 之后
            return false; //跳过原方法
        }
        return true; //其他稀有度走原方法
    }
}

//为 NobleCardPool 在卡牌总览中添加筛选按钮
//与 BaseLib 的 CustomPoolFilters 使用相同的 Transpiler 注入方式：
//在 _Ready 中 _regentFilter 初始化后插入 GenerateNobleFilter 调用，
//直接使用 _Ready 的 local_0（UpdateCardPoolFilter 的 Callable），不走反射
[HarmonyPatch(typeof(NCardLibrary), nameof(NCardLibrary._Ready))]
static class NoblePoolFilterPatch
{
    public static NCardPoolFilter? NobleFilter;

    [HarmonyTranspiler]
    static List<CodeInstruction> AddNobleFilter(IEnumerable<CodeInstruction> instructions)
    {
        return new InstructionPatcher(instructions)
            .Match(new InstructionMatcher()
                .ldfld(AccessTools.DeclaredField(typeof(NCardLibrary), "_regentFilter"))
                .callvirt(null)
            )
            .Insert([
                CodeInstruction.LoadArgument(0),                                    // this (NCardLibrary)
                CodeInstruction.LoadArgument(0),                                    // this
                new CodeInstruction(OpCodes.Ldfld,
                    AccessTools.DeclaredField(typeof(NCardLibrary), "_poolFilters")),// _poolFilters
                CodeInstruction.LoadArgument(0),                                    // this
                new CodeInstruction(OpCodes.Ldfld,
                    AccessTools.DeclaredField(typeof(NCardLibrary), "_rarityFilters")),// _rarityFilters
                CodeInstruction.LoadLocal(0),                                       // local_0 = updateFilter Callable
                CodeInstruction.Call(typeof(NoblePoolFilterPatch), nameof(GenerateNobleFilter))
            ]);
    }

    public static void GenerateNobleFilter(
        NCardLibrary library,
        Dictionary<NCardPoolFilter, Func<CardModel, bool>> filtering,
        Dictionary<NCardRarityTickbox, Func<CardModel, bool>> rarityFilters,
        Callable updateFilter)
    {
        if (filtering.Count == 0) return;
        if (filtering.Last().Key.GetParentControl() is not GridContainer parent) return;

        //--- 创建筛选按钮 ---
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

        parent.AddChild(filter);
        NobleFilter = filter;

        //--- 注册过滤函数 ---
        filtering.Add(filter, c => c.Pool is NobleCardPool);

        //--- 修改稀有度过滤：NobleCardPool 的卡总是通过稀有度筛选 ---
        var keys = rarityFilters.Keys.ToList();
        foreach (var key in keys)
        {
            var originalFunc = rarityFilters[key];
            rarityFilters[key] = c => c.Pool is NobleCardPool || originalFunc(c);
        }

        //--- 信号连接：使用 _Ready 的 local_0 Callable（和 BaseLib 完全一致）---
        filter.Connect(NCardPoolFilter.SignalName.Toggled, updateFilter);

        FieldInfo lastHoveredField = AccessTools.DeclaredField(typeof(NCardLibrary), "_lastHoveredControl");
        filter.Connect(Control.SignalName.FocusEntered,
            Callable.From(() => lastHoveredField.SetValue(library, filter)));
    }
}

//使 Noble 卡池被视为特殊池（与 misc/ancients 相同），从而正确禁用稀有度筛选
//并处理 Noble 的单选按钮行为（因为基础方法只识别 misc/ancients 为特殊池）
[HarmonyPatch(typeof(NCardLibrary), "UpdateCardPoolFilter")]
static class NobleRarityPatch
{
    private static readonly FieldInfo PoolFiltersField =
        AccessTools.DeclaredField(typeof(NCardLibrary), "_poolFilters");
    private static readonly FieldInfo RarityFiltersField =
        AccessTools.DeclaredField(typeof(NCardLibrary), "_rarityFilters");

    [HarmonyPrefix]
    static bool HandleNoblePoolSelection(NCardLibrary __instance)
    {
        var nobleFilter = NoblePoolFilterPatch.NobleFilter;
        if (nobleFilter == null || !nobleFilter.IsSelected) return true;

        //--- 单选按钮逻辑：选中 Noble 时，取消所有其他池 ---
        var poolFilters = (Dictionary<NCardPoolFilter, Func<CardModel, bool>>)PoolFiltersField.GetValue(__instance)!;
        foreach (var key in poolFilters.Keys)
            if (key != nobleFilter) key.IsSelected = false;

        //--- 禁用所有稀有度 tickbox（与 misc/ancients 行为一致）---
        var rarityFilters = (Dictionary<NCardRarityTickbox, Func<CardModel, bool>>)RarityFiltersField.GetValue(__instance)!;
        foreach (var tickbox in rarityFilters.Keys)
            tickbox.Disable();

        //--- 调用 UpdateFilter 刷新卡牌显示 ---
        AccessTools.Method(typeof(NCardLibrary), "UpdateFilter", [typeof(bool)])
            .Invoke(__instance, [false]);

        return false; //跳过基底方法
    }

    //当卡牌总览关闭时，清空静态引用，防止生命周期残留
    [HarmonyPostfix]
    [HarmonyPatch(typeof(NCardLibrary), nameof(NCardLibrary.OnSubmenuClosed))]
    static void ClearNobleFilterOnClose()
    {
        NoblePoolFilterPatch.NobleFilter = null;
    }
}
