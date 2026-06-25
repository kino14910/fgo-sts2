using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class DreamUponTheStars : FgoCard
{
    public DreamUponTheStars() : base(2, CardType.Skill,
        CardRarity.Basic, TargetType.Self)
    {
        WithBlock(8, 3);
        WithPower<NpDamagePower>(20);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.ApplySelf<NpDamagePower>(choiceContext, play.Card);
    }
}