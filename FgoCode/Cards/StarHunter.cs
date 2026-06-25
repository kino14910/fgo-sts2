using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class StarHunter : FgoCard
{
    public StarHunter() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithStar(8);
        WithPower<CriticalDamageOncePower>(50, 50);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await FgoStarCmd.AddStars(DynamicVars["Star"].IntValue);
        await CommonActions.ApplySelf<CriticalDamageOncePower>(choiceContext, play.Card);
    }
}