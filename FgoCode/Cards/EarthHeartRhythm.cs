using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class EarthHeartRhythm : FgoCard
{
    public EarthHeartRhythm() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithNp(20);
        WithPower<NpDamagePower>(20, 10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
        await CommonActions.ApplySelf<NpDamagePower>(choiceContext, play.Card);
    }
}