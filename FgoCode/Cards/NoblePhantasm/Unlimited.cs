using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class Unlimited : NobleCard
{
    public Unlimited() : base(1, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithVar("AttacksPerTurn", 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<UnlimitedPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["AttacksPerTurn"].BaseValue);
    }
}