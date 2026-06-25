using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class EnumaElish : NobleCard
{
    public EnumaElish() : base(2, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(32, 8);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
    }
}