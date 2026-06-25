using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class Insanity : FgoCard
{
    public Insanity() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithTags(FgoEnums.Foreigner);
        WithVar("NPOnCurseRemove", 10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<InsanityPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["NPOnCurseRemove"].BaseValue);
    }
}