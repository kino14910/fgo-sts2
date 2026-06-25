using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Relics;

public class WingBootsFgo : FgoRelic
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    public override async Task BeforeCombatStart()
    {
        Flash();
        await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(),
            Owner.Creature, 1m, Owner.Creature, null);
    }
}
