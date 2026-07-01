using BaseLib.Abstracts;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

/// <summary>
/// 一次性暴击威力 Power，叠加次数。
/// 每层提供 200% 暴击伤害，每次暴击消耗一层，层数归零后移除。
/// </summary>
public class CriticalDamageOncePower : CriticalDamagePower, IHasSecondAmount
{
    public int Amount2 { get; set; }
    private const decimal OnceCritMultiplier = 1m; // 200% = 1 + 1

    public override PowerStackType StackType => PowerStackType.Counter;
    public CriticalDamageOncePower(){}
    public CriticalDamageOncePower(Creature owner, int critPower, int stacks)
    {
        Owner = owner;
        Amount = stacks;
        Amount2 = critPower;
        CritTriggered = false;
    }
    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        CritTriggered = true;
        return base.ModifyDamageMultiplicative(target, amount, props, dealer, cardSource) + Amount2 / 100m; // 200%
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result,
        ValueProp props, Creature target, CardModel? cardSource)
    {
        if (Owner != dealer) return;
        if (!props.IsPoweredAttack()) return;
        if (result.TotalDamage <= 0) return;
        Flash();
        await PowerCmd.Decrement(this);
    }
    public string GetSecondAmount()
    {
        return Amount2.ToString();
    }
}
