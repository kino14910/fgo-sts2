using BaseLib.Abstracts;
using BaseLib.Extensions;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Powers;

public abstract class FgoTemporaryPower : CustomTemporaryPowerModel
{
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();

    public override AbstractModel OriginModel => this;
    public override PowerModel InternallyAppliedPower => ModelDb.Power<StrengthPower>();

    protected override Func<PlayerChoiceContext, Creature, decimal, Creature?, CardModel?, bool, Task> ApplyPowerFunc =>
        (ctx, target, amt, src, srcCard, silent) => Task.CompletedTask;

    protected virtual bool RemoveAtEndOfTurn => true;
    protected virtual bool RemoveAtStartOfTurn => false;

    protected virtual Task OnBeforeRemove(PlayerChoiceContext choiceContext)
    {
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!RemoveAtEndOfTurn) return;
        if (side != CombatSide.Player) return;
        Flash();
        await OnBeforeRemove(choiceContext);
        await PowerCmd.Remove(this);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!RemoveAtStartOfTurn) return;
        await OnBeforeRemove(choiceContext);
        await PowerCmd.Remove(this);
    }
}