using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

public class ReducePercentDamagePower : FgoTemporaryPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "def_power.png".PowerImagePath();
    public override string CustomBigIconPath => "def_power.png".BigPowerImagePath();

    protected override bool RemoveAtEndOfTurn => false;
    protected override bool RemoveAtStartOfTurn => true;

    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner) return 1m;
        return (100m - Amount) / 100m;
    }
}