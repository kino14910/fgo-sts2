using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class MaidensWillpower : FgoCard
{
    public MaidensWillpower() : base(1, CardType.Power,
        CardRarity.Common, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithPower<RegenPower>(5);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<RegenPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<ArtifactPower>(choiceContext, play.Card, 1m);
    }
}