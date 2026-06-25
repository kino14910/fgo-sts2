using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Powers;

public class AtTheWellPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "after_duration_power.png".PowerImagePath();
    public override string CustomBigIconPath => "after_duration_power.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        await PowerCmd.Apply<GutsPower>(choiceContext, Owner, Amount, Owner, null);
        await FgoNpCmd.AddNp(80);
        await PowerCmd.Remove(this);
    }
}