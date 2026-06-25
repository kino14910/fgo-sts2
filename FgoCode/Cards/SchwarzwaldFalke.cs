using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class SchwarzwaldFalke : FgoCard
{
    public SchwarzwaldFalke() : base(3, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithPower<RegenPower>(3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<NpRatePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["RegenPower"].BaseValue);
        await CommonActions.Apply<SchwarzwaldFalkePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["RegenPower"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["RegenPower"].UpgradeValueBy(2m);
    }
}