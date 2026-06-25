using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Relics;

public class BB : FgoRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;

    public override async Task BeforeCombatStart()
    {
        Flash();
        var self = Owner.Creature;
        var roll = Random.Shared.Next(2);

        if (roll == 0)
        {
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), self, 2m, self, null);
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), self, 2m, self, null);
        }
        else
        {
            var debuffRoll = Random.Shared.Next(3);
            if (debuffRoll == 0)
                await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), self, 1m, self, null);
            else if (debuffRoll == 1)
                await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), self, 1m, self, null);
            else
                await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), self, 1m, self, null);
        }
    }
}