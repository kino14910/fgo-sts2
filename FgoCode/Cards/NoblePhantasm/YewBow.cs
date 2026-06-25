using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class YewBow : NobleCard
{
    public YewBow() : base(1, CardType.Attack, TargetType.AnyEnemy)
    {
        WithDamage(6, 3);
    }

    protected override bool ShouldGlowGoldInternal =>
        CombatState?.HittableEnemies.Any(e => e.HasPower<PoisonPower>()) ?? false;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target!, vfx: "vfx/vfx_attack_bow").Execute(choiceContext);

        if (play.Target!.HasPower<PoisonPower>())
            await CommonActions.CardAttack(this, play.Target!, vfx: "vfx/vfx_attack_bow").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}