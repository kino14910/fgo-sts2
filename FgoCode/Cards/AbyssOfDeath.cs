using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class AbyssOfDeath : FgoCard
{
    public AbyssOfDeath() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithPower<RegenPower>(3, 2);
        WithPower<DeathOfDeathPower>(5, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<RegenPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<DeathOfDeathPower>(choiceContext, play.Card);
    }
}