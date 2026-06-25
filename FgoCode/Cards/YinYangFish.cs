using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class YinYangFish : FgoCard
{
    public YinYangFish() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithHeal(6, 2);
        WithNp(10);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await FgoNpCmd.AddNp(-DynamicVars["NP"].IntValue);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue, false);
    }
}