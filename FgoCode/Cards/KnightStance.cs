using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class KnightStance : FgoCard
{
    public KnightStance() : base(1, CardType.Skill,
        CardRarity.Common, TargetType.Self)
    {
        WithBlock(11, 3);
        WithVar("DamageReduction", 25);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.Apply<ReducePercentDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["DamageReduction"].BaseValue);
        await CommonActions.ApplySelf<NpRatePower>(choiceContext, play.Card, 1m);
    }
}