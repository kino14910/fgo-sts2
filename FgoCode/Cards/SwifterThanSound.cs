using BaseLib.Utils;
using Fgo.FgoCode.Cards.Colorless;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Cards;

public class SwifterThanSound : FgoCard
{
    public SwifterThanSound() : base(1, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithDamage(6, 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await PlayerCmd.GainEnergy(1m, Owner);
        await FgoCardActions.AddToPile(ModelDb.Card<InfiniteSuffering>().ToMutable(), PileType.Hand);
    }
}