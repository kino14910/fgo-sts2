using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class OneWhoWishesforSalvation : FgoCard
{
    public OneWhoWishesforSalvation() : base(1, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<NpPerTurnPower>(10, 5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<NpPerTurnPower>(choiceContext, play.Card);
    }
}