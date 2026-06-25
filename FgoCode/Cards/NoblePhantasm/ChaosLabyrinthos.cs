using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class ChaosLabyrinthos : NobleCard
{
    public ChaosLabyrinthos() : base(2, CardType.Skill, TargetType.Self)
    {
        WithPower<StrengthPower>(3, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<StrengthPower>(choiceContext, enemy, play.Card,
                -DynamicVars.Strength.BaseValue);
    }
}