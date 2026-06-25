using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class GraceUnexpectedBirth : FgoCard
{
    public GraceUnexpectedBirth() : base(0, CardType.Skill,
        CardRarity.Common, TargetType.Self)
    {
        WithPower<SealNpPower>(1);
        WithNp(30);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
        await CommonActions.ApplySelf<SealNpPower>(choiceContext, play.Card);
    }
}