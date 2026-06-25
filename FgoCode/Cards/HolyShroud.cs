using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class HolyShroud : FgoCard
{
    public HolyShroud() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ReducePercentDamagePower>(20);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<ReducePercentDamagePower>(choiceContext, play.Card);
    }
}