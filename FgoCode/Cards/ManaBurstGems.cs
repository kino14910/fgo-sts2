using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class ManaBurstGems : FgoCard
{
    public ManaBurstGems() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<StrengthPower>(3, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<ManaBurstGemsPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars.Strength.BaseValue);
    }
}