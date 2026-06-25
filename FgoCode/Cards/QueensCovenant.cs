using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class QueensCovenant : FgoCard
{
    public QueensCovenant() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<StrengthPower>(2, 1);
        WithPower<DexterityPower>(2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var strAmount = DynamicVars.Strength.BaseValue;

        await CommonActions.ApplySelf<StrengthPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<DexterityPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<MyFairSoldierPower>(choiceContext, play.Card, strAmount);
    }
}