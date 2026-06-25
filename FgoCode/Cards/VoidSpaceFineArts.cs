using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class VoidSpaceFineArts : FgoCard
{
    public VoidSpaceFineArts() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithTags(FgoEnums.Foreigner);
        WithPower<RegenPower>(10);
        WithVar("CurseStacks", 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var self = Owner.Creature;
        await CommonActions.ApplySelf<RegenPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<CursePower>(choiceContext, play.Card, 3m);
        var curseCount = self.GetPowerAmount<CursePower>();
        await FgoNpCmd.AddNp((int)DynamicVars["CurseStacks"].BaseValue * curseCount);
    }
}