using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class DefendFgo : FgoCard
{
    public DefendFgo() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithBlock(6, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }
}