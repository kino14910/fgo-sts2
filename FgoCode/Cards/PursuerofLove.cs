using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class PursuerofLove : FgoCard
{
    public PursuerofLove() : base(1, CardType.Skill,
        CardRarity.Common, TargetType.AnyEnemy)
    {
        WithPower<StrengthPower>(1);
        WithVar("Pursue", 2, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<StrengthPower>(choiceContext, play.Target!, play.Card);
        await CommonActions.Apply<WeakPower>(choiceContext, play.Target!, play.Card, DynamicVars["Pursue"].BaseValue);
    }
}