using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class WarriorsBlade : FgoCard
{
    public WarriorsBlade() : base(1, CardType.Attack,
        CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(2);
        WithVar("Hits", 4, 1);
        WithStar(6);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target!, DynamicVars["Hits"].IntValue, "vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await FgoStarCmd.AddStars(6);
    }
}