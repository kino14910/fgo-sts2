using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class LastSunXibalba : NobleCard
{
    public LastSunXibalba() : base(3, CardType.Attack, TargetType.Self)
    {
        WithDamage(6, 2);
        WithStar(10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, 5, "vfx/vfx_attack_fire").Execute(choiceContext);
        await FgoStarCmd.AddStars(DynamicVars.Stars.IntValue);
    }
}