using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class TrueClash : FgoCard
{
    public TrueClash() : base(1, CardType.Attack,
        CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(8, 3);
        WithBlock(6, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target!, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActions.CardBlock(this, play);
    }
}