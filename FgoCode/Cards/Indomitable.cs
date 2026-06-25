using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class Indomitable : FgoCard
{
    public Indomitable() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithPower<NonStackableGutsPower>(3, 2);
        WithPower<IndomitablePower>(2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<NonStackableGutsPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<IndomitablePower>(choiceContext, play.Card);
    }
}