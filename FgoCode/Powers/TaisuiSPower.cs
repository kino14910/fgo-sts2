using MegaCrit.Sts2.Core.Entities.Powers;

namespace Fgo.FgoCode.Powers;

public class TaisuiSPower : FgoPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
}