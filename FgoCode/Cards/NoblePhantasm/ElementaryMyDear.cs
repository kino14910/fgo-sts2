using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class ElementaryMyDear : NobleCard
{
    public ElementaryMyDear() : base(1, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithPower<VulnerablePower>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<ElementaryPower>(choiceContext, Owner.Creature, play.Card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(1m);
    }
}
