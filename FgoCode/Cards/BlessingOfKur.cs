using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class BlessingOfKur : FgoCard
{
    public BlessingOfKur() : base(1, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("TempHP", 6, 3);
        WithPower<StrengthPower>(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<NpRatePower>(choiceContext, play.Card, 3m);
        await CommonActions.Apply<BlessingOfKurPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["TempHP"].BaseValue);
    }
}