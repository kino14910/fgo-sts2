using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards;

public class CharismaOfConflict : FgoCard
{
    public CharismaOfConflict() : base(2, CardType.Attack,
        CardRarity.Common, TargetType.Self)
    {
        WithDamage(8, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);

        if (Owner.Creature.HasPower<RegenPower>())
            await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
    }
}