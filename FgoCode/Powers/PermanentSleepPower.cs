using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

public class PermanentSleepPower : FgoPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string CustomPackedIconPath => "permanent_sleep_power.png".PowerImagePath();
    public override string CustomBigIconPath => "permanent_sleep_power.png".BigPowerImagePath();

    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (dealer != Owner) return 1m;
        if (!props.HasFlag(ValueProp.Move)) return 1m;
        return 2m;
    }
}