using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Powers;

public class NpPerTurnPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "np_per_turn_power.png".PowerImagePath();
    public override string CustomBigIconPath => "np_per_turn_power.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        await FgoNpCmd.AddNp(Amount);
        await PowerCmd.Decrement(this);
    }
}