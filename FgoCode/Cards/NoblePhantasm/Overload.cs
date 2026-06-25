using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class Overload : NobleCard
{
    public Overload() : base(2, CardType.Attack, TargetType.AnyEnemy)
    {
        WithDamage(10, 4);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var self = Owner.Creature;
        // 宝具值获取提升
        await CommonActions.ApplySelf<NpRatePower>(choiceContext, play.Card, 1m);

        // 去除敌人的格挡值
        if (play.Target!.Block > 0) await CreatureCmd.LoseBlock(play.Target!, play.Target.Block);

        await CommonActions.CardAttack(this, play.Target!, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}