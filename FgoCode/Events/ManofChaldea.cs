using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Fgo.FgoCode.Events;

public class ManofChaldea : CustomEventModel
{
    public override List<(string, string)> Localization => new EventLoc("ManofChaldea",
        new EventPageLoc("INITIAL", "As humanity is the continuous footprint...",
            new EventOptionLoc("CONTINUE", "[Continue]", "")),
        new EventPageLoc("PAGE0", "At the end of the day, his memories are renewed...",
            new EventOptionLoc("CONTINUE2", "[Continue]", "")),
        new EventPageLoc("PAGE1", "And so, the boy became an adult...",
            new EventOptionLoc("GOLD", "[Continue] Gain 75 Gold.", ""),
            new EventOptionLoc("HEAL", "[Continue] Heal 15 HP.", "")),
        new EventPageLoc("GOLD", "You gain some gold."),
        new EventPageLoc("HEAL", "You heal some HP.")
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(75),
        new HealVar(15)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            Option(Continue)
        ];
    }

    public async Task Continue()
    {
        SetEventState(PageDescription("PAGE0"),
        [
            Option(Continue2)
        ]);
    }

    public async Task Continue2()
    {
        SetEventState(PageDescription("PAGE1"),
        [
            Option(Gold),
            Option(Heal)
        ]);
    }

    public async Task Gold()
    {
        await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner!);
        SetEventFinished(PageDescription("GOLD"));
    }

    public async Task Heal()
    {
        await CreatureCmd.Heal(Owner!.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(PageDescription("HEAL"));
    }
}