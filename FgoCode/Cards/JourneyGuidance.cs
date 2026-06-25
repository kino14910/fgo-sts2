using BaseLib.Utils;
using Fgo.FgoCode.Cards.Colorless;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Cards;

public class JourneyGuidance : FgoCard
{
    public JourneyGuidance() : base(2, CardType.Attack,
        CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(14, 4);
    }

    protected override bool ShouldGlowGoldInternal =>
        Owner.Creature.HasPower<WatersidePower>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_blunt")
            .Execute(choiceContext);

        if (Owner.Creature.HasPower<WatersidePower>()) await PlayerCmd.GainEnergy(2m, Owner);
        var channelCard = ModelDb.Card<SoulOfWaterChannels>().ToMutable();
        await CardPileCmd.Add(channelCard, PileType.Hand, CardPilePosition.Top, this);
    }
}