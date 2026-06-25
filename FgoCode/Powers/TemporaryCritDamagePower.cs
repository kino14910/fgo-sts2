using Fgo.FgoCode.Cards;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Fgo.FgoCode.Powers;

public class TemporaryCritDamagePower : FgoTemporaryPowerModelWrapper<HeroCreation, CriticalDamagePower>
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "after_duration_power.png".PowerImagePath();
    public override string CustomBigIconPath => "after_duration_power.png".BigPowerImagePath();

    public int CritDamageAmount { get; set; }
}