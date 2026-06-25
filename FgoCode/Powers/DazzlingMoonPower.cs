using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Powers;

public class DazzlingMoonPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "every_turn_debuff_power.png".PowerImagePath();
    public override string CustomBigIconPath => "every_turn_debuff_power.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, -1m, Owner, null);
        var combatState = Owner.CombatState;
        if (combatState != null)
            foreach (var enemy in combatState.HittableEnemies)
                await PowerCmd.Apply<StrengthPower>(choiceContext, enemy, -2m, Owner, null);

        await PowerCmd.Decrement(this);
    }

    public override async Task AfterRemoved(Creature owner)
    {
        var str = owner.GetPowerAmount<StrengthPower>();
        if (str < 0)
        {
            var abs = -str;
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, abs * 2m, Owner, null);
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), Owner, abs, Owner, null);
        }
    }
}