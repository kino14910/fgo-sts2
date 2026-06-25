using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class LakeTexcoco : FgoCard
{
    public LakeTexcoco() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithNp(20, 10);
        WithPower<NpPerTurnPower>(10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
        await CommonActions.ApplySelf<WatersidePower>(choiceContext, play.Card, 1m);
        await CommonActions.ApplySelf<NpPerTurnPower>(choiceContext, play.Card);
    }
}