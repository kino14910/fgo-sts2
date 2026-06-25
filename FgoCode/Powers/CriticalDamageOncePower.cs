using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

public class CriticalDamageOncePower : FgoTemporaryPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "critical_damage_power.png".PowerImagePath();
    public override string CustomBigIconPath => "critical_damage_power.png".BigPowerImagePath();
    public override bool AllowNegative => true;

    protected override bool RemoveAtEndOfTurn => false;

    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (Owner != dealer) return 1m;
        if (!props.IsPoweredAttack()) return 1m;
        return 1m + Amount / 100m;
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result,
        ValueProp props, Creature target, CardModel? cardSource)
    {
        if (Owner != dealer) return;
        if (!props.IsPoweredAttack()) return;
        if (result.TotalDamage <= 0) return;
        Flash();
        await PowerCmd.Remove(this);
    }
}