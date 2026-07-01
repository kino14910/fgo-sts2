using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards;

public class CharismaOfTheJade : FgoCard
{
    public CharismaOfTheJade() : base(2, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(7);
        WithVar("Hits", 3, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target!, DynamicVars["Hits"].IntValue, "vfx/vfx_attack_slash").Execute(choiceContext);
        // 暴击由 CriticalDamagePower 处理：Stars>=20 时消耗20星造成300%伤害，否则按基础暴击（Stars>=10消耗10星200%）
    }
}