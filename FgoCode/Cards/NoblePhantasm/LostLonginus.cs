using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class LostLonginus : NobleCard
{
    public LostLonginus() : base(2, CardType.Attack, TargetType.Self)
    {
        WithDamage(24, 6);
        WithPower<RegenPower>(6, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // 无敌贯通
        await CommonActions.ApplySelf<IgnoresInvincibilityPower>(choiceContext, play.Card, 1m);

        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);

        // 获得再生
        await CommonActions.ApplySelf<RegenPower>(choiceContext, play.Card);
    }
}