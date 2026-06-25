using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

public class CriticalDamagePower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "critical_damage_power.png".PowerImagePath();
    public override string CustomBigIconPath => "critical_damage_power.png".BigPowerImagePath();
    public override bool AllowNegative => true;

    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (Owner != dealer) return 1m;
        if (!props.IsPoweredAttack()) return 1m;

        // Return multiplier: 1 + (critDamage% / 100)
        return 1m + Amount / 100m;
    }
}