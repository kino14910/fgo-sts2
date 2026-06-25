using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class DepartureOfTheSun : FgoCard
{
    public DepartureOfTheSun() : base(1, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithTags(FgoEnums.Foreigner);
        WithVar("StarThreshold", 10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<DepartureOfTheSunPower>(choiceContext, play.Card, 1m);
        var power = Owner.Creature.GetPower<DepartureOfTheSunPower>();
        if (power != null)
            power.StarThreshold = DynamicVars["StarThreshold"].IntValue;
    }
}