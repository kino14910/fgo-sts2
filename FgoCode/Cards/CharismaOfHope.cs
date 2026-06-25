using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class CharismaOfHope : FgoCard
{
    public CharismaOfHope() : base(1, CardType.Attack,
        CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(6, 3);
        WithNp(20);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
        await CommonActions.CardAttack(this, play.Target!, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
    }
}