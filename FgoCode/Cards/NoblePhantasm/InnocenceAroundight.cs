using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class InnocenceAroundight : NobleCard
{
    public InnocenceAroundight() : base(2, CardType.Attack,
        CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(32, 8);
        WithPower<NpRatePower>(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        await CommonActions.ApplySelf<NpRatePower>(choiceContext, play.Card);
        await FgoCardActions.AddToPile(FgoCardActions.CreateCard<RayHorizon>(true), PileType.Draw);
    }
}