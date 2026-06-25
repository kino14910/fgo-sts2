using Fgo.FgoCode.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class EnferChateaudIf : NobleCard
{
    public EnferChateaudIf() : base(1, CardType.Power, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        // Grant WingBoots relic if not already owned
        var hasRelic = Owner.Relics.Any(r => r is WingBootsFgo);
        if (!hasRelic)
            await RelicCmd.Obtain(ModelDb.Relic<WingBootsFgo>().ToMutable(), Owner, Owner.Relics.Count);
    }
}