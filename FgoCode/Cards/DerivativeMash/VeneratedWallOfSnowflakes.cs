using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.DerivativeMash;

public class VeneratedWallOfSnowflakes : FgoColorlessCard
{
    public VeneratedWallOfSnowflakes() : base(1, CardType.Skill,
        CardRarity.Token, TargetType.Self)
    {
        WithBlock(10, 10);
        WithVar("DamageReduction", 20, 10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.Apply<ReducePercentDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["DamageReduction"].BaseValue);
    }
}