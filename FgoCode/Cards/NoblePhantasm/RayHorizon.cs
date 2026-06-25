using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class RayHorizon : NobleCard
{
    public RayHorizon() : base(0, CardType.Skill,
        FgoEnums.NoblePhantasm, TargetType.Self)
    {
        WithNp(50, 50);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        FgoCardActions.ForceNextNpCard(Owner);
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue, choiceContext, Owner);
        await CommonActions.ApplySelf<InvincibilityTurnPower>(choiceContext, play.Card, 1m);
    }
}