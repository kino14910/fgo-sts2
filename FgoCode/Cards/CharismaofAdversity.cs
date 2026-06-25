using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Fgo.FgoCode.Cards;

public class CharismaOfAdversity : FgoCard
{
    public CharismaOfAdversity() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(6, 3);
        WithVars(
            new CalculationBaseVar(1m),
            new CalculationExtraVar(1m),
            new CalculatedVar("CalculatedHits").WithMultiplier((card, _) =>
                (decimal)Math.Floor(
                    (card.Owner.Creature.MaxHp - card.Owner.Creature.CurrentHp) / 6.0)));
    }

    protected override bool ShouldGlowGoldInternal =>
        Owner.Creature.MaxHp - Owner.Creature.CurrentHp >= 12;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var hits = (int)((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(play.Target);

        await CommonActions.CardAttack(this, play.Target!, hits, "vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
    }
}