using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class ItsInevitable : FgoCard
{
    public ItsInevitable() : base(1, CardType.Attack,
        CardRarity.Common, TargetType.Self)
    {
        WithDamage(4, 1);
        WithVar("Boost", 4, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_fire").Execute(choiceContext);
        await CommonActions.Apply<ItsInevitablePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars.Damage.IntValue);
        var power = Owner.Creature.GetPower<ItsInevitablePower>();
        if (power != null)
            power.Boost = DynamicVars["Boost"].IntValue;
    }
}