using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class FetchFailnaught : NobleCard
{
    public FetchFailnaught() : base(1, CardType.Attack, TargetType.AnyEnemy)
    {
        WithDamage(30, 8);
        WithVar("CurseMultiplier", 1, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var target = play.Target!;
        // 伤害提高：敌人每有一层诅呪+10%
        var bonus = 1m;
        if (target.HasPower<CursePower>())
            bonus += target.GetPowerAmount<CursePower>() * 0.1m;

        await CommonActions
            .CardAttack(this, play.Target!, DynamicVars.Damage.BaseValue * bonus, vfx: "vfx/vfx_attack_blunt")
            .Execute(choiceContext);

        await CommonActions.Apply<CursePower>(choiceContext, target, play.Card, 3m);
        if (target.HasPower<CursePower>())
        {
            var curAmt = target.GetPowerAmount<CursePower>();
            await CommonActions.Apply<CursePower>(choiceContext, target, play.Card,
                curAmt * DynamicVars["CurseMultiplier"].IntValue);
        }
    }
}