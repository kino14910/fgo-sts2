using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class BridalSpinWheel : NobleCard
{
    public BridalSpinWheel() : base(1, CardType.Skill, TargetType.Self)
    {
        WithPower<PlatingPower>(2, 1);
        WithStar(8, 4);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<PlatingPower>(choiceContext, play.Card);
        await FgoStarCmd.AddStars(DynamicVars["Star"].IntValue);
    }
}