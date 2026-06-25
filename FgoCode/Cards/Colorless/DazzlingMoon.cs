using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.Colorless;

public class DazzlingMoon : FgoColorlessCard
{
    public DazzlingMoon() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithPower<StrengthPower>(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var self = Owner.Creature;
        await CommonActions.ApplySelf<DazzlingMoonPower>(choiceContext, play.Card, 3m);
    }

    protected override void OnUpgrade()
    {
        WithKeywords(CardKeyword.Innate);
    }
}