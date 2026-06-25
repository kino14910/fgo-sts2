using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class WildRule : FgoCard
{
    public WildRule() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(10, 4);
        WithPower<VulnerablePower>(1);
    }

    protected override bool ShouldGlowGoldInternal =>
        CombatState?.HittableEnemies.Any(e => e.GetPowerAmount<StrengthPower>() > 0) ?? false;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_bite")
            .Execute(choiceContext);

        await CreatureCmd.Heal(Owner.Creature, 3m, false);

        if (play.Target?.GetPowerAmount<StrengthPower>() > 0)
        {
            await CommonActions.Apply<StrengthPower>(choiceContext, play.Target!, play.Card, -1m);
            await CommonActions.Apply<VulnerablePower>(choiceContext, play.Target!, play.Card);
        }
    }
}