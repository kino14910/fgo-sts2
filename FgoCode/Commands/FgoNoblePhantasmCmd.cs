using Fgo.FgoCode.Cards.DerivativeMash;
using Fgo.FgoCode.Cards.NoblePhantasm;
using Fgo.FgoCode.Character;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Relics;
using Fgo.FgoCode.Singletons;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Fgo.FgoCode.Commands;

public static class FgoNoblePhantasmCmd
{
    public static async Task<bool> TryChooseNoblePhantasm(PlayerChoiceContext choiceContext, Player player)
    {
        var resources = ModelDb.Singleton<FgoPlayerResources>();
        if (!resources.CanUseNp) return false;
        if (player.Creature.HasPower<SealNpPower>()) return false;

        var overCharge = resources.OverCharge;
        var forcedCard = FgoCardActions.TakeForcedNpCard(player);
        if (forcedCard != null)
        {
            await AddNobleCardToHand(forcedCard, player, overCharge);
            resources.SpendNpForNoblePhantasm();
            return true;
        }

        var cards = ModelDb.CardPool<NobleCardPool>()
            .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
            .OfType<NobleCard>()
            .Where(card => IsMashNpCardAvailable(card, player))
            .Select(card => PrepareNobleCard(card, player, overCharge))
            .ToList();

        if (cards.Count == 0) return false;

        var prefs = new CardSelectorPrefs(new LocString("ui", "FGO-NP_TEXT.text_2"), 1)
        {
            RequireManualConfirmation = true
        };
        var selected = (await CardSelectCmd.FromSimpleGrid(choiceContext, cards, player, prefs)).FirstOrDefault();
        if (selected == null) return false;

        await CardPileCmd.AddGeneratedCardToCombat(selected, PileType.Hand, player, CardPilePosition.Top);
        resources.SpendNpForNoblePhantasm();
        return true;
    }

    private static async Task AddNobleCardToHand(NobleCard card, Player player, int overCharge)
    {
        var copy = PrepareNobleCard(card, player, overCharge);
        await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, player, CardPilePosition.Top);
    }

    private static NobleCard PrepareNobleCard(NobleCard source, Player player, int overCharge)
    {
        var card = (NobleCard)source.ToMutable();
        card.Owner = player;
        for (var i = 0; i < overCharge && card.IsUpgradable; i++)
            CardCmd.Upgrade(card, CardPreviewStyle.None);
        return card;
    }

    /// <summary>
    ///     根据马修卡牌强化等级过滤宝具卡。
    ///     Level 0: Camelot
    ///     Level 1: LordCamelot + ObscurantWallOfChalk
    ///     Level 2: LordChaldeas + ObscurantWallOafChalk
    /// </summary>
    private static bool IsMashNpCardAvailable(NobleCard card, Player player)
    {
        var relic = player.Relics.OfType<BlessedLockOnChocolate>().FirstOrDefault();
        var level = relic?.MashUpgradeLevel ?? 0;

        return card switch
        {
            Camelot => level == 0,
            LordCamelot => level == 1,
            LordChaldeas => level == 2,
            ObscurantWallofChalk => level >= 1,
            _ => true
        };
    }
}