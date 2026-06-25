using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.DerivativeMash;

public class RayProofKyrielight : FgoColorlessCard
{
    public RayProofKyrielight() : base(1, CardType.Attack,
        CardRarity.Status, TargetType.Self)
    {
        WithDamage(30, 10);
        WithPower<VulnerablePower>(3, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_heavy").Execute(choiceContext);
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await CommonActions.Apply<VulnerablePower>(choiceContext, enemy, play.Card);
            if (!enemy.IsPrimaryEnemy)
            {
                var buffs = enemy.Powers.Where(power => power.Type == PowerType.Buff).ToList();
                foreach (var buff in buffs)
                    await PowerCmd.Remove(buff);
            }

            await CommonActions.Apply<IgnoresInvincibilityPower>(choiceContext, enemy, play.Card, 1m);
        }
    }
}