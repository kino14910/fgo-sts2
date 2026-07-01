using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class Desterrennacht : NobleCard
{
    public Desterrennacht() : base(2, CardType.Power, TargetType.Self)
    {
        WithTags(FgoEnums.Foreigner);
        WithPower<StrengthPower>(2, 1);
        WithVar("CritDamage", 60, 10);
        WithVar("TerrorChance", 60);
        WithVar("StarRegen", 10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        // 给予所有敌人3层恐怖(60%概率)
        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<DoomPower>(choiceContext, enemy, play.Card,
                DynamicVars["TerrorChance"].BaseValue);

        await CommonActions.ApplySelf<StrengthPower>(choiceContext, play.Card);
        await CommonActions.Apply<CriticalDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["CritDamage"].BaseValue);
        await CommonActions.Apply<StarsPerTurnPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["StarRegen"].BaseValue);
    }
}