using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class MonteCristoTreasure : FgoCard
{
    public MonteCristoTreasure() : base(3, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithVar("Multiplier", 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<MonteCristoTreasurePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["Multiplier"].BaseValue);
    }
}