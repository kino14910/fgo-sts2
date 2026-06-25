using Fgo.FgoCode.Singletons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Powers;

public class SealNpPower : FgoTemporaryPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override bool RemoveAtEndOfTurn => false;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        await PowerCmd.TickDownDuration(this);
        await ModelDb.Singleton<FgoPlayerResources>().ResolvePendingNoblePhantasmSelection(choiceContext, player);
    }
}