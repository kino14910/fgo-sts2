using Fgo.FgoCode.Cards.Colorless.OptionCards;
using Fgo.FgoCode.Singletons;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Commands;

public static class FgoCommandSpellCmd
{
    private const int HealAmount = 30;
    private const int NpAmount = 100;

    public static async Task<bool> TryUseCommandSpell(PlayerChoiceContext choiceContext, Player player)
    {
        var resources = ModelDb.Singleton<FgoPlayerResources>();
        if (!resources.CanUseCommandSpell) return false;

        var cards = new List<CardModel>
        {
            ModelDb.Card<RepairSpiritOrigin>().ToMutable(),
            ModelDb.Card<ReleaseNoblePhantasm>().ToMutable()
        };

        var prefs = new CardSelectorPrefs(new LocString("ui", "FGO-COMMAND_SPELL.text_3"), 1)
        {
            RequireManualConfirmation = true
        };

        var selected = (await CardSelectCmd.FromSimpleGrid(choiceContext, cards, player, prefs)).FirstOrDefault();
        if (selected == null) return false;

        resources.UseCommandSpell();

        if (selected is RepairSpiritOrigin)
            await CreatureCmd.Heal(player.Creature, HealAmount);
        else if (selected is ReleaseNoblePhantasm) await resources.AddNp(NpAmount, choiceContext, player);

        return true;
    }
}