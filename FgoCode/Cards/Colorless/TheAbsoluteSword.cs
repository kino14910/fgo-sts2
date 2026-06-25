using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless;

public class TheAbsoluteSword : FgoColorlessCard
{
    public TheAbsoluteSword() : base(3, CardType.Attack,
        CardRarity.Token, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithDamage(10, 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        for (var i = 0; i < 2; i++)
            await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, 1m, Owner);
        await PlayerCmd.GainEnergy(3m, Owner);
        await FgoNpCmd.AddNp(30);
    }
}