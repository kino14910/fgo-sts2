using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless;

public class FifthForce : FgoColorlessCard
{
    public FifthForce() : base(1, CardType.Skill,
        CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithVar("DamageReduction", 50);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<FifthForcePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["DamageReduction"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DamageReduction"].BaseValue = 35m;
    }
}