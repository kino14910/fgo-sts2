using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class WorldsEndFlowerGarden : FgoCard
{
    public WorldsEndFlowerGarden() : base(2, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("NPOnCrit", 10, 5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<WorldsEndFlowerGardenPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["NPOnCrit"].BaseValue);
    }
}