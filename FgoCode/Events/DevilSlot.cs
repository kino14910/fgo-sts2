using BaseLib.Abstracts;
using Fgo.FgoCode.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Events;

public class DevilSlot : CustomEventModel
{
    public override List<(string, string)> Localization => new EventLoc("DevilSlot",
        new EventPageLoc("INITIAL", "This is the worst-case scenario, you have no other choice.",
            new EventOptionLoc("BB", "[Devil's Gift] Obtain BB.", ""),
            new EventOptionLoc("REMOVE", "[Outrun] Remove a card from your deck.", "")),
        new EventPageLoc("BB", "You don't want the gift anymore."),
        new EventPageLoc("REMOVE", "It's quite a coincidence.")
    );

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            Option(BB).WithRelic<BB>(Owner!),
            Option(RemoveCard)
        ];
    }

    public async Task BB()
    {
        var relic = ModelDb.Relic<BB>().ToMutable();
        await RelicCmd.Obtain(relic, Owner!);
        SetEventFinished(PageDescription("BB"));
    }

    public async Task RemoveCard()
    {
        // 从主牌组中选择一张卡牌移除（非基础、非诅咒卡牌）
        var removableCards = Owner!.Deck.Cards
            .Where(c => c.Rarity != CardRarity.Basic && c.Type != CardType.Curse)
            .ToList();

        if (removableCards.Count > 0)
        {
            var prefs = new CardSelectorPrefs(new LocString("ui", "DevilSlot.text_remove"), 1)
            {
                RequireManualConfirmation = true
            };
            var selected = (await CardSelectCmd.FromSimpleGrid(
                new ThrowingPlayerChoiceContext(), removableCards, Owner!, prefs)).FirstOrDefault();

            if (selected != null) await CardPileCmd.RemoveFromDeck(selected);
        }

        SetEventFinished(PageDescription("REMOVE"));
    }
}