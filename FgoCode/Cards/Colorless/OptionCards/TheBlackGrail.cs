using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless.OptionCards;

public class TheBlackGrail : FgoColorlessCard
{
    public TheBlackGrail() : base(0, CardType.Skill,
        CardRarity.Token, TargetType.Self)
    {
        WithPower<NpDamagePower>(30, 20);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<NpDamagePower>(choiceContext, play.Card);
    }
}