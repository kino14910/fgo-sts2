using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class SecondLife : NobleCard
{
    public SecondLife() : base(1, CardType.Skill,
        CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var exhaustCards = Owner.PlayerCombatState!.ExhaustPile.Cards.ToList();
        if (exhaustCards.Count == 0) return;

        var card = exhaustCards[Random.Shared.Next(exhaustCards.Count)];
        var copy = card.ToMutable();
        if (copy.IsUpgradable)
            CardCmd.Upgrade(copy, CardPreviewStyle.None);
        await FgoCardActions.AddToPile(copy, PileType.Hand);
    }
}
