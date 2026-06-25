using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class SwordOfSelection : FgoCard
{
    public SwordOfSelection() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(2, 1);
        WithNp(20);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Draw(this, choiceContext);
        await FgoNpCmd.AddNp(FgoCardActions.HandSize(Owner) * DynamicVars["NP"].IntValue / 10);
    }
}