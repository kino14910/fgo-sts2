using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Fgo.FgoCode.Relics;

public class SuitcaseFgo : FgoRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override async Task BeforeCombatStart()
    {
        Flash();
        await FgoNpCmd.AddNp(20);
    }
}