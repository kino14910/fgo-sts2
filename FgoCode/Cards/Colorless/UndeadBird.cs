using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless;

public class UndeadBird : FgoColorlessCard
{
    public UndeadBird() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithVar("Cards", 3, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var cards = CommonActions.GenerateCards(this, DynamicVars["Cards"].IntValue,
                card => card.DynamicVars.ContainsKey("H"))
            .Select(card => card.ToMutable())
            .ToList();
        if (cards.Count > 0)
            await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner, CardPilePosition.Top);
    }
}