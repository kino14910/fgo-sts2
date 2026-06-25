using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class LuckySpinning : FgoCard
{
    public LuckySpinning() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithStar(1, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<LuckySpinningPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["Star"].BaseValue);
    }
}