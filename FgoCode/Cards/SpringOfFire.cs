using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class SpringOfFire : FgoCard
{
    public SpringOfFire() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithHeal(20, 10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<NonStackableGutsPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars.Heal.BaseValue);
    }
}