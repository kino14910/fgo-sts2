using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless;

public class SoulOfWaterChannels : FgoCard
{
    public SoulOfWaterChannels() : base(0, CardType.Skill,
        CardRarity.Token, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust, CardKeyword.Retain);
        WithTags(FgoEnums.Foreigner);
        WithVar("Stars", 10, 5);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<CursePower>(choiceContext, play.Card, 1m);
        await FgoStarCmd.AddStars(DynamicVars["Stars"].IntValue);
    }
}