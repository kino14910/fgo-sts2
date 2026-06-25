using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class TeslaCoil : FgoCard
{
    public TeslaCoil() : base(1, CardType.Attack,
        CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(8, 3);
        WithPower<RegenPower>(2);
        WithPower<NpRatePower>(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CommonActions.ApplySelf<RegenPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<NpRatePower>(choiceContext, play.Card);
    }
}