using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class FromTheWorldsEnd : FgoCard
{
    public FromTheWorldsEnd() : base(2, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<WeakPower>(3);
        WithVar("Turns", 3, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<WeakPower>(choiceContext, enemy, play.Card);
        await CommonActions.Apply<FromTheWorldsEndPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["Turns"].BaseValue);
    }
}