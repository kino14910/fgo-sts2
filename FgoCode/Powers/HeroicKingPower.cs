using Fgo.FgoCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

public class HeroicKingPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.TotalDamage <= 0) return;

        var player = Owner.Player;
        if (player?.PlayerCombatState?.DiscardPile?.Cards == null) return;

        var card = player.PlayerCombatState.DiscardPile.Cards
            .FirstOrDefault(card => card is HeroicKing);
        if (card == null) return;

        Flash();
        await CardPileCmd.Add(card, PileType.Hand, CardPilePosition.Top, this, true);
        await PowerCmd.Decrement(this);
    }
}