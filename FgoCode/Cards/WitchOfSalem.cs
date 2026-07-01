using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class WitchOfSalem : FgoCard
{
    public WitchOfSalem() : base(2, CardType.Skill,
        CardRarity.Rare, TargetType.Self)
    {
        WithTags(FgoEnums.Foreigner);
        WithPower<VulnerablePower>(3);
        WithPower<WeakPower>(3);
        WithVar("TerrorChance", 30);
        WithVar("VsTerrorDamage", 50);
        WithNp(20);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await CommonActions.Apply<VulnerablePower>(choiceContext, enemy, play.Card);
            await CommonActions.Apply<WeakPower>(choiceContext, enemy, play.Card);
            await CommonActions.Apply<DoomPower>(choiceContext, enemy, play.Card,
                DynamicVars["TerrorChance"].BaseValue);
        }

        await CommonActions.Apply<CriticalDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["VsTerrorDamage"].BaseValue);
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
    }
}