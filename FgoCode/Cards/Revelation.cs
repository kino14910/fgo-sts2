using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class Revelation : FgoCard
{
    public Revelation() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithPower<VulnerablePower>(1, 1);
        WithPower<WeakPower>(1, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<VulnerablePower>(choiceContext, play.Target!, play.Card);
        await CommonActions.Apply<WeakPower>(choiceContext, play.Target!, play.Card);
    }
}