using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.Colorless;

public class PoisonousDagger : FgoColorlessCard
{
    public PoisonousDagger() : base(0, CardType.Attack,
        CardRarity.Token, TargetType.AnyEnemy)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithDamage(2);
        WithPower<PoisonPower>(2, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CommonActions.Apply<PoisonPower>(choiceContext, play.Target!, play.Card);
    }
}