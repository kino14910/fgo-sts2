using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Extensions;
using Fgo.FgoCode.Singletons;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Fgo.FgoCode.UI;

public static class FgoCombatUi
{
    private const string UiPanelName = "FgoNpUi";

    private static readonly Color NpGaugeBg = new("1a1a2e");
    private static readonly Color NpGaugeFillActive = new("00e676");
    private static readonly Color NpGaugeFillDefault = new("2979ff");
    private static readonly Color NpReadyGlow = new("00e676");
    private static readonly Color TextWhite = new("ffffff");
    private static readonly Color Gold = new("f3ca49");
    private static readonly Color CritRed = new("ff4444");

    private static readonly string NpButtonPath = "np_button.png".CharacterUiPath();

    private static StyleBoxFlat? _npFillStyleActive;
    private static StyleBoxFlat? _npFillStyleDefault;

    public static void AttachTo(NCreatureVisuals visuals)
    {
        var existing = visuals.GetNodeOrNull<Control>(UiPanelName);
        existing?.QueueFree();

        var panel = new Control();
        panel.Name = UiPanelName;
        panel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopLeft);

        var bounds = visuals.GetNodeOrNull<Control>("%Bounds");
        var offsetY = bounds != null ? bounds.Position.Y - 50f : -330f;
        var centerX = bounds != null ? bounds.Position.X + bounds.Size.X * 0.5f : 0f;

        var vbox = new VBoxContainer();
        vbox.Name = "NpVBox";
        vbox.Position = new Vector2(centerX - 100f, offsetY);
        vbox.Size = new Vector2(200f, 100f);
        vbox.AddThemeConstantOverride("separation", 4);
        panel.AddChild(vbox);

        BuildNpGauge(vbox);
        BuildStarDisplay(vbox);
        BuildNpButton(vbox);
        BuildCommandSpellButton(vbox);

        visuals.AddChild(panel);
        panel.Owner = visuals;

        UpdateAll();
    }

    private static void BuildNpGauge(VBoxContainer parent)
    {
        var container = new HBoxContainer();
        container.Name = "NpGaugeContainer";
        container.AddThemeConstantOverride("separation", 6);
        parent.AddChild(container);

        var npLabel = new Label();
        npLabel.Name = "NpLabel";
        npLabel.Text = "NP";
        npLabel.AddThemeFontSizeOverride("font_size", 16);
        npLabel.AddThemeColorOverride("font_color", Gold);
        container.AddChild(npLabel);

        var bar = new ProgressBar();
        bar.Name = "NpBar";
        bar.MinValue = 0;
        bar.MaxValue = 100;
        bar.Value = 0;
        bar.ShowPercentage = false;
        bar.CustomMinimumSize = new Vector2(120f, 20f);
        bar.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        var bgStyle = new StyleBoxFlat();
        bgStyle.BgColor = NpGaugeBg;
        bgStyle.SetCornerRadiusAll(4);
        bgStyle.ContentMarginLeft = 2;
        bgStyle.ContentMarginRight = 2;
        bar.AddThemeStyleboxOverride("background", bgStyle);

        var fillStyle = new StyleBoxFlat();
        fillStyle.BgColor = NpGaugeFillDefault;
        fillStyle.SetCornerRadiusAll(4);
        fillStyle.ContentMarginLeft = 2;
        fillStyle.ContentMarginRight = 2;
        bar.AddThemeStyleboxOverride("fill", fillStyle);

        container.AddChild(bar);

        var valueLabel = new Label();
        valueLabel.Name = "NpValueLabel";
        valueLabel.Text = "0%";
        valueLabel.AddThemeFontSizeOverride("font_size", 14);
        valueLabel.AddThemeColorOverride("font_color", TextWhite);
        valueLabel.CustomMinimumSize = new Vector2(40f, 0);
        container.AddChild(valueLabel);
    }

    private static void BuildStarDisplay(VBoxContainer parent)
    {
        var container = new HBoxContainer();
        container.Name = "StarContainer";
        container.AddThemeConstantOverride("separation", 4);
        parent.AddChild(container);

        var starIcon = new Label();
        starIcon.Name = "StarIcon";
        starIcon.Text = "★";
        starIcon.AddThemeFontSizeOverride("font_size", 16);
        starIcon.AddThemeColorOverride("font_color", Gold);
        container.AddChild(starIcon);

        var starLabel = new Label();
        starLabel.Name = "StarLabel";
        starLabel.Text = "0";
        starLabel.AddThemeFontSizeOverride("font_size", 14);
        starLabel.AddThemeColorOverride("font_color", TextWhite);
        container.AddChild(starLabel);

        var critLabel = new Label();
        critLabel.Name = "CritLabel";
        critLabel.Text = "CRIT!";
        critLabel.AddThemeFontSizeOverride("font_size", 14);
        critLabel.AddThemeColorOverride("font_color", CritRed);
        critLabel.Visible = false;
        container.AddChild(critLabel);
    }

    private static void BuildNpButton(VBoxContainer parent)
    {
        var button = new TextureButton();
        button.Name = "NpButton";
        button.CustomMinimumSize = new Vector2(120f, 36f);
        button.IgnoreTextureSize = true;
        button.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;

        if (ResourceLoader.Exists(NpButtonPath))
            button.TextureNormal = GD.Load<Texture2D>(NpButtonPath);

        button.Pressed += OnNpButtonPressed;
        button.Visible = false;
        parent.AddChild(button);

        var label = new Label();
        label.Name = "NpButtonLabel";
        label.Text = "NP!";
        label.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.VerticalAlignment = VerticalAlignment.Center;
        label.AddThemeFontSizeOverride("font_size", 16);
        label.AddThemeColorOverride("font_color", TextWhite);
        label.MouseFilter = Control.MouseFilterEnum.Ignore;
        button.AddChild(label);
    }

    private static string GetCommandSpellImagePath(int count)
    {
        return $"ui/CommandSpell/CommandSpell{Math.Clamp(count, 0, 3)}.png".ImagePath();
    }

    private static void BuildCommandSpellButton(VBoxContainer parent)
    {
        var button = new TextureButton();
        button.Name = "CommandSpellButton";
        button.CustomMinimumSize = new Vector2(64f, 64f);
        button.IgnoreTextureSize = true;
        button.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;

        var path = GetCommandSpellImagePath(3);
        if (ResourceLoader.Exists(path))
            button.TextureNormal = GD.Load<Texture2D>(path);

        button.Pressed += OnCommandSpellButtonPressed;
        parent.AddChild(button);
    }

    public static void UpdateAll()
    {
        try
        {
            var resources = ModelDb.Singleton<FgoPlayerResources>();
            var nodes = FindUiNodes().ToList();
            UpdateNpGauge(nodes, resources.Np, resources.CanUseNp);
            UpdateStars(nodes, resources.Stars, resources.CanCrit);
            UpdateNpButton(nodes, resources.CanUseNp);
            UpdateCommandSpell(nodes, resources.CommandSpell);
        }
        catch (Exception e)
        {
            // FgoPlayerResources not yet initialized; skip but log for diagnostics
            MainFile.Logger.Debug($"FgoCombatUi.UpdateAll skipped: {e.Message}");
        }
    }

    private static StyleBoxFlat GetNpFillStyle(bool canUse)
    {
        if (canUse)
        {
            _npFillStyleActive ??= CreateNpFillStyle(NpGaugeFillActive);
            return _npFillStyleActive;
        }

        _npFillStyleDefault ??= CreateNpFillStyle(NpGaugeFillDefault);
        return _npFillStyleDefault;
    }

    private static StyleBoxFlat CreateNpFillStyle(Color color)
    {
        var style = new StyleBoxFlat();
        style.BgColor = color;
        style.SetCornerRadiusAll(4);
        style.ContentMarginLeft = 2;
        style.ContentMarginRight = 2;
        return style;
    }

    private static void UpdateNpGauge(IReadOnlyList<Control> nodes, int np, bool canUse)
    {
        foreach (var node in nodes)
        {
            var bar = node.GetNodeOrNull<ProgressBar>("NpVBox/NpGaugeContainer/NpBar");
            if (bar != null)
            {
                bar.Value = Math.Min(np, 100);
                bar.AddThemeStyleboxOverride("fill", GetNpFillStyle(canUse));
            }

            var valueLabel = node.GetNodeOrNull<Label>("NpVBox/NpGaugeContainer/NpValueLabel");
            if (valueLabel != null)
            {
                valueLabel.Text = $"{np}%";
                valueLabel.AddThemeColorOverride("font_color", canUse ? NpReadyGlow : TextWhite);
            }
        }
    }

    private static void UpdateStars(IReadOnlyList<Control> nodes, int stars, bool canCrit)
    {
        foreach (var node in nodes)
        {
            var starLabel = node.GetNodeOrNull<Label>("NpVBox/StarContainer/StarLabel");
            if (starLabel != null)
            {
                starLabel.Text = stars.ToString();
                starLabel.AddThemeColorOverride("font_color", canCrit ? Gold : TextWhite);
            }

            var critLabel = node.GetNodeOrNull<Label>("NpVBox/StarContainer/CritLabel");
            if (critLabel != null)
                critLabel.Visible = canCrit;
        }
    }

    private static void UpdateNpButton(IReadOnlyList<Control> nodes, bool canUse)
    {
        foreach (var node in nodes)
        {
            var button = node.GetNodeOrNull<TextureButton>("NpVBox/NpButton");
            if (button != null)
                button.Visible = canUse;
        }
    }

    private static void UpdateCommandSpell(IReadOnlyList<Control> nodes, int count)
    {
        var path = GetCommandSpellImagePath(count);
        var tex = ResourceLoader.Exists(path) ? GD.Load<Texture2D>(path) : null;

        foreach (var node in nodes)
        {
            var button = node.GetNodeOrNull<TextureButton>("NpVBox/CommandSpellButton");
            if (button != null)
            {
                if (tex != null) button.TextureNormal = tex;
                button.Modulate = count > 0 ? new Color(1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.4f);
            }
        }
    }

    private static async void OnNpButtonPressed()
    {
        try
        {
            var resources = ModelDb.Singleton<FgoPlayerResources>();
            if (!resources.CanUseNp) return;

            resources.SetNpButtonPressed();

            try
            {
                var state = CombatManager.Instance.DebugOnlyGetState();
                var player = LocalContext.GetMe(state) ?? state?.Players.FirstOrDefault();
                if (player != null)
                    await FgoNoblePhantasmCmd.TryChooseNoblePhantasm(new ThrowingPlayerChoiceContext(), player);
            }
            catch (Exception e)
            {
                MainFile.Logger.Error($"NP button async call failed: {e}");
            }
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"NP button error: {e}");
        }
    }

    private static async void OnCommandSpellButtonPressed()
    {
        try
        {
            var resources = ModelDb.Singleton<FgoPlayerResources>();
            if (!resources.CanUseCommandSpell) return;

            var state = CombatManager.Instance.DebugOnlyGetState();
            var player = LocalContext.GetMe(state) ?? state?.Players.FirstOrDefault();
            if (player != null)
                await FgoCommandSpellCmd.TryUseCommandSpell(new ThrowingPlayerChoiceContext(), player);
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"CommandSpell button error: {e}");
        }
    }

    private static IEnumerable<Control> FindUiNodes()
    {
        var tree = (SceneTree?)Engine.GetMainLoop();
        var root = tree?.Root;
        if (root == null) yield break;

        foreach (var found in FindUiInChildren(root))
            yield return found;
    }

    private static IEnumerable<Control> FindUiInChildren(Node node)
    {
        if (node is NCreatureVisuals visuals)
        {
            var ui = visuals.GetNodeOrNull<Control>(UiPanelName);
            if (ui != null) yield return ui;
        }

        foreach (var child in node.GetChildren())
        foreach (var found in FindUiInChildren(child))
            yield return found;
    }
}