using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class EndOfADream : FgoCard
{
    public EndOfADream() : base(1, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithVar("ExhaustCount", 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<PermanentSleepPower>(choiceContext, play.Card, 1m);
    }
}