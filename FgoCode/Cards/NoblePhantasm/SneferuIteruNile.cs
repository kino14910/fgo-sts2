using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class SneferuIteruNile : NobleCard
{
    public SneferuIteruNile() : base(2, CardType.Attack, TargetType.Self)
    {
        WithDamage(35, 10);
        WithPower<VulnerablePower>(3);
        WithVar("DeathChance", 12, 2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await CommonActions.Apply<VulnerablePower>(choiceContext, enemy, play.Card, 3m);
            await CommonActions.Apply<DoomPower>(choiceContext, enemy, play.Card,
                DynamicVars["DeathChance"].BaseValue);
        }

        await CommonActions.ApplySelf<WatersidePower>(choiceContext, play.Card, 1m);
    }
}