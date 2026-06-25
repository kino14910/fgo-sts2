using BaseLib.Abstracts;
using Fgo.FgoCode.Cards.Colorless;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Events;

public class Beyondthe : CustomEventModel
{
    public override List<(string, string)> Localization => new EventLoc("Beyondthe",
        new EventPageLoc("INITIAL", "Every path I took led me to a darkness as dead as night...",
            new EventOptionLoc("CONTINUE", "[Continue]", "")),
        new EventPageLoc("PHASE_1", "I have to live. I need to put myself together...",
            new EventOptionLoc("SHVIBZIK", "[Continue] Get a Shvibzik.", ""),
            new EventOptionLoc("MAXHP", "[Kadoc] Gain Max HP.", "")),
        new EventPageLoc("LEAVE", "You continue walking towards the top of the tower.")
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("MaxHpgain", 6m)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            Option(Continue),
            Option(Shvibzik),
            Option(MaxHp)
        ];
    }

    public async Task Continue()
    {
        SetEventState(PageDescription("PHASE_1"),
        [
            Option(Shvibzik),
            Option(MaxHp)
        ]);
    }

    public async Task Shvibzik()
    {
        var card = ModelDb.Card<Shvibzik>().ToMutable();
        await CardPileCmd.Add(card, PileType.Hand);
        SetEventFinished(PageDescription("LEAVE"));
    }

    public async Task MaxHp()
    {
        await CreatureCmd.GainMaxHp(Owner!.Creature, DynamicVars["MaxHpgain"].IntValue);
        SetEventFinished(PageDescription("LEAVE"));
    }
}