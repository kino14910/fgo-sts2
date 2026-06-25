using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Relics;

public class FanNight : FgoRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task AfterBlockBroken(Creature creature)
    {
        if (creature == Owner.Creature) return;
        Flash();
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), creature, 2m, Owner.Creature, null);
    }
}