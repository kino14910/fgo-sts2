using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Extensions;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class ImaginaryOceanBattle : FgoCard
{
    public ImaginaryOceanBattle() : base(1, CardType.Skill,
        CardRarity.Rare, TargetType.Self)
    {
        WithNp(5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var npAmount = DynamicVars.Np().IntValue + Owner.RunState.TotalFloor;
        await FgoNpCmd.AddNp(npAmount);
        await CommonActions.ApplySelf<ImaginarySpacePower>(choiceContext, play.Card, 1m);
        await CommonActions.ApplySelf<WatersidePower>(choiceContext, play.Card, 1m);
    }

    protected override void OnUpgrade()
    {
        WithCostUpgradeBy(-1);
    }
}