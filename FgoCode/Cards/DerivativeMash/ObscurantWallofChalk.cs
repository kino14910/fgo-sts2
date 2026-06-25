using BaseLib.Utils;
using Fgo.FgoCode.Cards.NoblePhantasm;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.DerivativeMash;

public class ObscurantWallofChalk : NobleCard
{
    public ObscurantWallofChalk() : base(0, CardType.Skill,
        FgoEnums.NoblePhantasm, TargetType.Self)
    {
        WithNp(30, 20);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<AntiPurgeDefensePower>(choiceContext, play.Card, 1m);
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
    }
}