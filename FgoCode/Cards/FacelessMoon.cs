using BaseLib.Utils;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class FacelessMoon : FgoCard
{
    public FacelessMoon() : base(1, CardType.Skill,
        CardRarity.Common, TargetType.Self)
    {
        WithTags(FgoEnums.Foreigner);
        WithBlock(5, 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        // Retain + block per retained handled by FacelessMoonPower
        await CommonActions.CardBlock(this, play);
    }
}