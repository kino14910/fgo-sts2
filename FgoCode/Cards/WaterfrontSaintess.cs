using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class WaterfrontSaintess : FgoCard
{
    public WaterfrontSaintess() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithNp(20, 10);
        WithPower<NpDamagePower>(20, 10);
        WithVar("CritDamage", 30);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
        await CommonActions.ApplySelf<NpDamagePower>(choiceContext, play.Card);
        if (Owner.Creature.HasPower<WatersidePower>())
            await CommonActions.Apply<CriticalDamagePower>(choiceContext, Owner.Creature, play.Card,
                DynamicVars["CritDamage"].BaseValue);
    }
}