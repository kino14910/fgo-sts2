using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class HeroCreation : FgoCard
{
    public HeroCreation() : base(0, CardType.Skill,
        CardRarity.Common, TargetType.Self)
    {
        WithPower<StrengthPower>(2, 2);
        WithVar("CriticalDamage", 50, 50);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var self = Owner.Creature;
        var strAmt = DynamicVars.Strength.BaseValue;
        var critAmt = DynamicVars["CriticalDamage"].BaseValue;

        await CommonActions.ApplySelf<StrengthPower>(choiceContext, play.Card);
        await CommonActions.Apply<CriticalDamagePower>(choiceContext, self, play.Card, critAmt);
        var tempPower = await CommonActions.Apply<TemporaryCritDamagePower>(choiceContext, self, play.Card, strAmt);
        if (tempPower != null) tempPower.CritDamageAmount = (int)critAmt;
    }
}