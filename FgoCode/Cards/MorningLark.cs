using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class MorningLark : FgoCard
{
    public MorningLark() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithNp(20);
        WithStar(8);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
        await FgoStarCmd.AddStars(DynamicVars["Star"].IntValue);
    }
}