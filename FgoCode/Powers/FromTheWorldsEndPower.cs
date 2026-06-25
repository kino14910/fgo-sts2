using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Powers;

public class FromTheWorldsEndPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "every_turn_debuff_power.png".PowerImagePath();
    public override string CustomBigIconPath => "every_turn_debuff_power.png".BigPowerImagePath();

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            var combatState = Owner.CombatState;
            if (combatState != null)
            {
                Flash();
                foreach (var enemy in combatState.HittableEnemies)
                    await PowerCmd.Apply<StrengthPower>(choiceContext, enemy, -1m, Owner, null);
            }

            await PowerCmd.Decrement(this);
        }
    }
}