using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Powers;

public class DeathOfDeathPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "trigger_after_attacks_power.png".PowerImagePath();
    public override string CustomBigIconPath => "trigger_after_attacks_power.png".BigPowerImagePath();

    
    public override async Task BeforeDeath(Creature creature)
    {
        if (creature != Owner || Amount <= 0) return;

        Flash();
        await ReviveCmd.Execute(creature, Amount);
        await CommonActions.ApplySelf<ShadowStepPower>(new ThrowingPlayerChoiceContext(), null, false);
    }
}