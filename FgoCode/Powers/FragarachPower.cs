using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

public class FragarachPower : FgoTemporaryPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "fragarach_counter_power.png".PowerImagePath();
    public override string CustomBigIconPath => "fragarach_counter_power.png".BigPowerImagePath();

    protected override bool RemoveAtEndOfTurn => false;
    protected override bool RemoveAtStartOfTurn => true;

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner) return;
        if (dealer == null || dealer == Owner) return;
        if (result.TotalDamage <= 0) return;
        Flash();
        await FgoStarCmd.AddStars(4);
        await CreatureCmd.Damage(choiceContext, dealer, Amount,
            ValueProp.Unpowered | ValueProp.Move, Owner);
    }
}