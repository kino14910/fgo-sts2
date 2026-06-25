using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class MaidenOfAFlowerPatio : FgoCard
{
    public MaidenOfAFlowerPatio() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(15, 5);
        WithPower<CursePower>(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.ApplySelf<CursePower>(choiceContext, play.Card);
    }
}