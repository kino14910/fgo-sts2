using Fgo.FgoCode.Singletons;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards;

public class ExtraterrestrialOctopus : FgoCard
{
    public ExtraterrestrialOctopus() : base(2, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithTags(FgoEnums.Foreigner);
        WithVar("StarMultiplier", 2, 1);
        WithCalculatedDamage(0, static (card, player) =>
        {
            var stars = card.FgoRes().Stars;
            var multiplier = card.DynamicVars["StarMultiplier"].IntValue;
            var totalDamage = stars * multiplier;
            return totalDamage;
        });
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, play.Target!, DynamicVars.CalculatedDamage.IntValue,
            ValueProp.Unpowered | ValueProp.Move, this);
    }
}