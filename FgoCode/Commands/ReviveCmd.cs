using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace Fgo.FgoCode.Commands;

public class ReviveCmd
{
    public static async Task Execute(Creature creature, int amount)
    {
        creature.CurrentHp = 0;
        await CreatureCmd.Heal(creature, amount);
        await PowerCmd.Remove<GutsPower>(creature);
    }
}