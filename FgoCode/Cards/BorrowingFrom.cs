using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Singletons;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class BorrowingFrom : FgoCard
{
    public BorrowingFrom() : base(1, CardType.Skill,
        CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var current = this.FgoRes().Np;
        await FgoNpCmd.AddNp(current);
    }
}