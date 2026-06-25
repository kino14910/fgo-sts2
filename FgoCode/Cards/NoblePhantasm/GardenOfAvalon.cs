using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class GardenOfAvalon : NobleCard
{
    public GardenOfAvalon() : base(3, CardType.Power, TargetType.Self)
    {
        WithBlock(3, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<NpPerTurnPower>(choiceContext, play.Card, 10m);
        await CommonActions.ApplySelf<StarsPerTurnPower>(choiceContext, play.Card, 3m);
    }
}