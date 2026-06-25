using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class RoadlessCamelot : NobleCard
{
    public RoadlessCamelot() : base(3, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(24, 8);
        WithVar("Curse", 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, 3, "vfx/vfx_attack_slash").Execute(choiceContext);

        // 给予所有敌人3层诅呪
        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<CursePower>(choiceContext, enemy, play.Card, DynamicVars["Curse"].BaseValue);
    }
}