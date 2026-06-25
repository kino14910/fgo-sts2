using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards;

public class SongOfThePoet : FgoCard
{
    public SongOfThePoet() : base(1, CardType.Attack,
        CardRarity.Common, TargetType.AnyEnemy)
    {
        WithVars(new DamageVar(6m, ValueProp.Move));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target!, 1, "vfx/vfx_attack_slash").Execute(choiceContext);
        // NP gain from unblocked damage handled by StarPower.AfterDamageGiven hook
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}