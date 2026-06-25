using BaseLib.Utils;
using Fgo.FgoCode.Cards.Colorless;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Cards;

public class Canopus : FgoCard
{
    public Canopus() : base(1, CardType.Attack,
        CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(15, 5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await FgoCardActions.AddToPile(ModelDb.Card<CurseDisaster>().ToMutable(), PileType.Discard);
    }
}