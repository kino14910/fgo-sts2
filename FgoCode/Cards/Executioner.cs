using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class Executioner : FgoCard
{
    public Executioner() : base(1, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(7, 4);
        WithPower<StrengthPower>(2);
        WithEnergy(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target!, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActions.Apply<StrengthPower>(choiceContext, play.Target!, play.Card);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }
}