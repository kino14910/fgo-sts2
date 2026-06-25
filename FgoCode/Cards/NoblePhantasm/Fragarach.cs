using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class Fragarach : NobleCard
{
    public Fragarach() : base(1, CardType.Power, TargetType.Self)
    {
        WithDamage(15, 5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<FragarachPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars.Damage.BaseValue);
    }
}