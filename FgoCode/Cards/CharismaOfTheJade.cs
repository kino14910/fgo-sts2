using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards;

public class CharismaOfTheJade : FgoCard
{
    public CharismaOfTheJade() : base(2, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(7);
        WithVar("Hits", 3, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target!, DynamicVars["Hits"].IntValue, "vfx/vfx_attack_slash").Execute(choiceContext);
        // Stars consumption for triple damage handled by FgoStarPower.ModifyDamageMultiplicative
    }
}