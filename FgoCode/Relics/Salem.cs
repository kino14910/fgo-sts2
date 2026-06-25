using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Relics;

public class Salem : FgoRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        var foreignerCount = Owner.Deck.Cards.Count(card => card.Tags.Contains(FgoEnums.Foreigner));
        if (foreignerCount > 0)
        {
            Flash();
            await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, foreignerCount * 2, Owner.Creature, null);
            await FgoStarCmd.AddStars(foreignerCount * 2);
        }
    }
}