using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Relics;

public class StargazerTeapot : FgoRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterPotionUsed(PotionModel potion, Creature? target)
    {
        Flash();
        await PowerCmd.Apply<GutsPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 2m, Owner.Creature, null);
    }
}