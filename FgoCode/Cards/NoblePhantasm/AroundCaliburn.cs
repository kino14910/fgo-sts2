using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class AroundCaliburn : NobleCard
{
    public AroundCaliburn() : base(2, CardType.Power, TargetType.Self)
    {
        WithPower<StrengthPower>(4);
        WithVar("AntiPurge", 1, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var self = Owner.Creature;
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, play.Card);
        await CommonActions.Apply<AntiPurgeDefensePower>(choiceContext, self, play.Card,
            DynamicVars["AntiPurge"].BaseValue);
    }
}