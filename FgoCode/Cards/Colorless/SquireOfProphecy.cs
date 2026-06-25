using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless;

public class SquireOfProphecy : FgoColorlessCard
{
    public SquireOfProphecy() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithVar("Turns", 2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<SquireOfProphecyPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["Turns"].BaseValue);
    }
}