using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards;

public class PeerlessStrike : FgoCard
{
    public PeerlessStrike() : base(2, CardType.Attack,
        CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithTags(CardTag.Strike);
        WithVars(new DamageVar(24m, ValueProp.Move), new IntVar("CritDamage", 100m));
    }

    protected override bool IsPlayable =>
        Owner.Creature.CurrentHp <= Owner.Creature.MaxHp / 2;

    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<CriticalDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["CritDamage"].BaseValue);
        await CommonActions.CardAttack(this, play.Target!, vfx: "vfx/vfx_attack_heavy").Execute(choiceContext);
        // 升级前立即死亡，升级后回合结束时死亡
        if (IsUpgraded)
            await CommonActions.ApplySelf<DeathChancePower>(choiceContext, play.Card, 100m);
        else
            await CreatureCmd.Kill(Owner.Creature);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m);
    }
}