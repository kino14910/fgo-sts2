using BaseLib.Abstracts;
using Fgo.FgoCode.Cards;
using Fgo.FgoCode.Cards.Colorless;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Fgo.FgoCode.Events;

public class ProofAndRebuttalEvent : CustomEventModel
{
    private const int UpgradeCount = 2;

    public override List<(string, string)> Localization => new EventLoc("ProofAndRebuttalEvent",
        new EventPageLoc("INITIAL", "Because it is a person who dedicates himself to the formula...",
            new EventOptionLoc("WITNESS", "[Witness] Get a Proof and Rebuttal.", ""),
            new EventOptionLoc("ENJOY", "[Enjoy] Upgrade 2 random cards. Lose !Gold! Gold.", ""),
            new EventOptionLoc("LOCKED", "[Locked] Requires: Upgradeable Cards.", "")),
        new EventPageLoc("WITNESS", "You choose to record this moment."),
        new EventPageLoc("ENJOY", "You really enjoy this moment.")
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(0)
    ];

    public override void CalculateVars()
    {
        DynamicVars.Gold.BaseValue = Rng.NextInt(40, 61);
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        // 检查牌组中是否有可升级的卡牌
        var hasUpgradableCards = Owner!.Deck.Cards.Any(c => c.IsUpgradable);

        if (hasUpgradableCards)
            return
            [
                Option(Witness),
                Option(Enjoy)
            ];

        return
        [
            Option(Witness),
            LockedOption("LOCKED")
        ];
    }

    public async Task Witness()
    {
        var card = ModelDb.Card<ProofAndRebuttal>().ToMutable();
        await CardPileCmd.Add(card, PileType.Hand);
        SetEventFinished(PageDescription("WITNESS"));
    }

    public async Task Enjoy()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner!);

        // 随机升级牌组中的 2 张卡牌
        var upgradableCards = Owner!.Deck.Cards
            .Where(c => c.IsUpgradable)
            .OrderBy(_ => Random.Shared.Next())
            .Take(UpgradeCount)
            .ToList();

        foreach (var card in upgradableCards)
            if (card.IsUpgradable)
                CardCmd.Upgrade(card, CardPreviewStyle.None);

        SetEventFinished(PageDescription("ENJOY"));
    }
}