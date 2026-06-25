using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class GoddessMetamorphosisBeast : FgoCard
{
    public GoddessMetamorphosisBeast() : base(2, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<PoisonPower>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<GoddessMetamorphosisBeastPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["PoisonPower"].BaseValue);
    }
}