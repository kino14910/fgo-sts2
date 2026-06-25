using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class CalamityOfTheNorth : FgoCard
{
    public CalamityOfTheNorth() : base(2, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Retain);
        WithPower<PoisonPower>(5, 3);
        WithPower<CursePower>(5, 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await CommonActions.Apply<PoisonPower>(choiceContext, enemy, play.Card);
            await CommonActions.Apply<CursePower>(choiceContext, enemy, play.Card);
        }
    }
}