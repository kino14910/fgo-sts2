using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class LieLikeVortigern : NobleCard
{
    public LieLikeVortigern() : base(3, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(25, 7);
        WithPower<IntangiblePower>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);

        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await CommonActions.Apply<StrengthPower>(choiceContext, enemy, play.Card, -2m);
            await CommonActions.Apply<IntangiblePower>(choiceContext, enemy, play.Card);
        }

        await CommonActions.ApplySelf<IntangiblePower>(choiceContext, play.Card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(7m);
    }
}