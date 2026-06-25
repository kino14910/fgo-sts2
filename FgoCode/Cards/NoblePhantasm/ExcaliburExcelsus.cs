using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class ExcaliburExcelsus : NobleCard
{
    public ExcaliburExcelsus() : base(3, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(16, 6);
        WithBlock(6);
        WithVar("Strength", 1);
        WithNp(10);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // 无敌贯通
        await CommonActions.ApplySelf<IgnoresInvincibilityPower>(choiceContext, play.Card, 1m);

        var enemyCount = CombatState!.HittableEnemies.Count();
        // 每有一名敌人，获得格挡、力量、宝具值
        if (enemyCount > 0)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars["Block"].BaseValue * enemyCount,
                ValueProp.Unpowered, play);
            await CommonActions.Apply<StrengthPower>(choiceContext, Owner.Creature, play.Card,
                DynamicVars["Strength"].BaseValue * enemyCount);
            await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue * enemyCount);
        }

        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
    }
}