using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.DerivativeMash;

public class VeneratedShieldOfSnowflakes : FgoColorlessCard
{
    public VeneratedShieldOfSnowflakes() : base(1, CardType.Skill,
        CardRarity.Token, TargetType.Self)
    {
        WithVar("DamageReduction", 20);
        WithNp(20);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var self = Owner.Creature;
        await CommonActions.Apply<ReducePercentDamagePower>(choiceContext, self, play.Card,
            DynamicVars["DamageReduction"].BaseValue);
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, play.Card, 3m);
        await CommonActions.ApplySelf<CriticalDamagePower>(choiceContext, play.Card, 30m);
    }
}