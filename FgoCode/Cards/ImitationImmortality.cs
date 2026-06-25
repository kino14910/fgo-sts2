using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class ImitationImmortality : FgoCard
{
    public ImitationImmortality() : base(2, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<NonStackableGutsPower>(5, 3);
        WithPower<PlatingPower>(6, 3);
        WithPower<NpPerTurnPower>(10, 5);
        WithVar("DamageReduction", 10, 5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<NonStackableGutsPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<PlatingPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<NpPerTurnPower>(choiceContext, play.Card);
        await CommonActions.Apply<ReducePercentDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["DamageReduction"].BaseValue);
    }
}