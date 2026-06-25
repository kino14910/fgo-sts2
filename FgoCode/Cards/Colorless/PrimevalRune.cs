using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.Colorless;

public class PrimevalRune : FgoColorlessCard
{
    public PrimevalRune() : base(1, CardType.Skill,
        CardRarity.Rare, TargetType.Self)
    {
        WithPower<WeakPower>(2);
        WithPower<VulnerablePower>(2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await CommonActions.Apply<WeakPower>(choiceContext, enemy, play.Card);
            await CommonActions.Apply<VulnerablePower>(choiceContext, enemy, play.Card);
        }
    }
}