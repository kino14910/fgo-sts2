using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class GodsExecution : FgoCard
{
    public GodsExecution() : base(2, CardType.Attack,
        CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(21, 7);
        WithPower<StrengthPower>(2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);

        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<StrengthPower>(choiceContext, enemy, play.Card,
                -DynamicVars.Strength.BaseValue);
    }
}