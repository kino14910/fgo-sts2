using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless;

public class TaisuiSMisfortune : FgoColorlessCard
{
    public TaisuiSMisfortune() : base(0, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust, CardKeyword.Innate);
        WithEnergy(2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PlayerCmd.GainEnergy(1m, Owner);
        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<TaisuiSPower>(choiceContext, enemy, play.Card,
                DynamicVars["Energy"].BaseValue);
    }
}