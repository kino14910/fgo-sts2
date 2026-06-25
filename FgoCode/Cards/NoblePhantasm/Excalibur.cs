using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class Excalibur : NobleCard
{
    public Excalibur() : base(2, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(25, 7);
        WithNp(30);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
    }
}