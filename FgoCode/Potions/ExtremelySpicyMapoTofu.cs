using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Potions;

public class ExtremelySpicyMapoTofu : FgoPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await FgoNpCmd.AddNp(40);
        await PowerCmd.Apply<NpDamagePower>(choiceContext, Owner.Creature, 40m, Owner.Creature, null);
    }
}