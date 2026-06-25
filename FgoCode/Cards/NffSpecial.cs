using BaseLib.Utils;
using Fgo.FgoCode.Cards.Colorless;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Cards;

public class NffSpecial : FgoCard
{
    public NffSpecial() : base(0, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Draw(this, choiceContext);
        await FgoCardActions.AddToPile(ModelDb.Card<PoisonousDagger>().ToMutable(), PileType.Discard);
    }
}