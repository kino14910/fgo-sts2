using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless;

public class GrandOrder : FgoColorlessCard
{
    public GrandOrder() : base(1, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(9999);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_heavy").Execute(choiceContext);
    }
}