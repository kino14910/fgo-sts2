using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class AntiBeastMagecraft : FgoCard
{
    public AntiBeastMagecraft() : base(1, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<AntiBeastMagecraftPower>(3, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<AntiBeastMagecraftPower>(choiceContext, play.Card);
    }
}