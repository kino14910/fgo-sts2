using BaseLib.Abstracts;
using Fgo.FgoCode.Cards;
using Fgo.FgoCode.Cards.NoblePhantasm;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.UI;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Singletons;

public class FgoPlayerResources() : CustomSingletonModel(HookType.Combat)
{
    private const int MaxCommandSpell = 3;
    private const int MaxOverCharge = 4;
    private const int BasicCritStars = 10;
    private const int SpecialCritStars = 20;

    private int _commandSpell;
    private int _np;
    private bool _npButtonPressed;
    private int _stars;

    public int Np
    {
        get => _np;
        private set => _np = Math.Clamp(value, 0, 300);
    }

    public int Stars
    {
        get => _stars;
        private set => _stars = Math.Max(0, value);
    }

    public int CommandSpell
    {
        get => _commandSpell;
        private set => _commandSpell = Math.Clamp(value, 0, MaxCommandSpell);
    }

    public bool CanCrit => Stars >= BasicCritStars;
    public bool CanUseNp => Np >= 100;
    public bool CanUseCommandSpell => CommandSpell > 0;
    public int OverCharge { get; private set; }

    public int OverChargeLevel => OverCharge;
    public bool HasPendingNoblePhantasmSelection { get; private set; }

    /// <summary>
    /// 尝试消耗暴击星。返回实际消耗的数量（0=未触发）。
    /// special=true 时按 SpecialCritStars(20) 消耗，否则按 BasicCritStars(10)。
    /// </summary>
    public int TryConsumeCritStars(bool special)
    {
        var required = special ? SpecialCritStars : BasicCritStars;
        if (Stars < required) return 0;
        ConsumeStars(required);
        return required;
    }

    public async Task AddNp(int amount, PlayerChoiceContext? choiceContext = null, Player? player = null)
    {
        var old = Np;
        Np += amount;
        if (Np == 99 && old < 99) Np = 100;

        SyncOverCharge(old, Np);

        if (old < 100 && Np >= 100)
            HasPendingNoblePhantasmSelection = true;

        if (choiceContext != null && player != null)
            await ResolvePendingNoblePhantasmSelection(choiceContext, player);

        FgoCombatUi.UpdateAll();
    }

    public Task<bool> ConsumeNp(int amount, Player? player = null)
    {
        if (Np < amount) return Task.FromResult(false);
        var old = Np;
        Np -= amount;
        SyncOverCharge(old, Np);
        return Task.FromResult(true);
    }

    public void GainOverCharge(int amount)
    {
        if (amount <= 0) return;
        OverCharge = Math.Clamp(OverCharge + amount, 0, MaxOverCharge);
    }

    public void SpendNpForNoblePhantasm()
    {
        _np = 0;
        FgoCombatUi.UpdateAll();
    }

    public async Task<bool> ResolvePendingNoblePhantasmSelection(PlayerChoiceContext choiceContext, Player player)
    {
        if (!HasPendingNoblePhantasmSelection) return false;
        if (!CanUseNp) return false;

        HasPendingNoblePhantasmSelection = false;
        if (await FgoNoblePhantasmCmd.TryChooseNoblePhantasm(choiceContext, player))
            return true;

        if (CanUseNp)
            HasPendingNoblePhantasmSelection = true;
        return false;
    }

    public void AddStars(int amount = 1)
    {
        Stars += amount;
        FgoCombatUi.UpdateAll();
    }

    public bool ConsumeStars(int amount)
    {
        if (Stars < amount) return false;
        Stars -= amount;
        FgoCombatUi.UpdateAll();
        return true;
    }

    public bool UseCommandSpell(int amount = 1)
    {
        if (CommandSpell < amount) return false;
        CommandSpell -= amount;
        FgoCombatUi.UpdateAll();
        return true;
    }

    public void ResetCommandSpell()
    {
        CommandSpell = MaxCommandSpell;
    }

    public void Reset()
    {
        _np = 0;
        _stars = 0;
        OverCharge = 0;
        _commandSpell = MaxCommandSpell;
        HasPendingNoblePhantasmSelection = false;
        _npButtonPressed = false;
        FgoCardActions.ClearAllForcedNpCards();
        FgoCombatUi.UpdateAll();
    }

    public void SetNpButtonPressed()
    {
        _npButtonPressed = true;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (_npButtonPressed && CanUseNp)
        {
            _npButtonPressed = false;
            var _ = CombatManager.Instance.DebugOnlyGetState();
            await ResolvePendingNoblePhantasmSelection(choiceContext, player);
        }
    }

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        return card is NobleCard ? playCount + OverCharge : playCount;
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
            OverCharge = 0;
        return Task.CompletedTask;
    }

    private static int OverChargeLevelFor(int np)
    {
        return np >= 300 ? 2 : np >= 200 ? 1 : 0;
    }

    private void SyncOverCharge(int oldNp, int newNp)
    {
        var delta = OverChargeLevelFor(newNp) - OverChargeLevelFor(oldNp);
        if (delta > 0)
            GainOverCharge(delta);
        else if (delta < 0)
            OverCharge = Math.Max(0, OverCharge + delta);
    }
}