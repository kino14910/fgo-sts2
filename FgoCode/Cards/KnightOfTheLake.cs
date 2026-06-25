using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class KnightOfTheLake : FgoCard
{
    public KnightOfTheLake() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("CritDamage", 30, 20);
        WithVar("Stars", 10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (!Owner.Creature.HasPower<CriticalDamagePower>())
            await CommonActions.Apply<CriticalDamagePower>(choiceContext, Owner.Creature, play.Card,
                DynamicVars["CritDamage"].BaseValue);
        else
            await FgoStarCmd.AddStars(DynamicVars["Stars"].IntValue);
    }
}