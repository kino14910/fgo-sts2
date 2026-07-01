using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Fgo.FgoCode.Powers;

public class GutsPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeDeath(Creature creature)
    {
        if (creature != Owner || Amount <= 0) return;

        Flash();
        await ReviveCmd.Execute(creature, Amount);
    }
}