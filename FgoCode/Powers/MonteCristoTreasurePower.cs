using Fgo.FgoCode.Singletons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

public class MonteCristoTreasurePower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext, Creature? dealer,
        DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != Owner) return;
        if (target == Owner) return;
        if (!props.HasFlag(ValueProp.Move)) return;
        if (!this.FgoRes().CanCrit) return;
        Flash();
        await CreatureCmd.GainBlock(Owner, result.TotalDamage * Amount, ValueProp.Unpowered, null);
    }
}