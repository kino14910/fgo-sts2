using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class ImitationGodForce : NobleCard
{
    public ImitationGodForce() : base(2, CardType.Attack, TargetType.Self)
    {
        WithDamage(8, 3);
        WithPower<WeakPower>(2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, 4, "vfx/vfx_attack_slash").Execute(choiceContext);

        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<WeakPower>(choiceContext, enemy, play.Card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}