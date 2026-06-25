using BaseLib.Utils;
using Fgo.FgoCode.Cards.NoblePhantasm;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.DerivativeMash;

public class LordCamelot : NobleCard
{
    public LordCamelot() : base(1, CardType.Power,
        CardRarity.Status, TargetType.Self)
    {
        WithPower<PlatingPower>(1, 1);
        WithVar("DamageReduction", 30, 20);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<PlatingPower>(choiceContext, play.Card);
        await CommonActions.Apply<ReducePercentDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["DamageReduction"].BaseValue);
    }
}