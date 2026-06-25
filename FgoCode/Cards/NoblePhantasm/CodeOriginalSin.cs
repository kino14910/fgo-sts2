using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class CodeOriginalSin : NobleCard
{
    public CodeOriginalSin() : base(1, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(40, 8);
        WithNp(20);
        WithPower<DoomPower>(8);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, CombatState!.HittableEnemies,
            DynamicVars.Damage.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            Owner.Creature);
        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<DoomPower>(choiceContext, enemy, play.Card);
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
    }
}