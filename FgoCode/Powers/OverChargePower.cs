using MegaCrit.Sts2.Core.Entities.Powers;

namespace Fgo.FgoCode.Powers;

public class OverChargePower : FgoTemporaryPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}