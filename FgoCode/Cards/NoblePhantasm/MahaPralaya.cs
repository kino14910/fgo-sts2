using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class MahaPralaya : NobleCard
{
    public MahaPralaya() : base(3, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(8, 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var debuffTypes = CombatState!.HittableEnemies
            .SelectMany(enemy => enemy.Powers)
            .Where(power => power.Type == PowerType.Debuff)
            .Select(power => power.Id)
            .Distinct()
            .Count();

        var hitCount = Math.Max(1, debuffTypes);
        await CommonActions.CardAttack(this, play, hitCount, "vfx/vfx_attack_blunt").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}