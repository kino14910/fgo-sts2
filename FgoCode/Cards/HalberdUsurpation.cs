using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards;

public class HalberdUsurpation : FgoCard
{
    public HalberdUsurpation() : base(2, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(new DamageVar(15m, ValueProp.Move));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var enemyStrength = play.Target!.GetPowerAmount<StrengthPower>();
        var totalDamage = (int)DynamicVars.Damage.BaseValue + enemyStrength;

        await CommonActions.CardAttack(this, play.Target!, totalDamage, "vfx/vfx_attack_heavy").Execute(choiceContext);

        if (enemyStrength > 0)
            await CommonActions.Apply<StrengthPower>(choiceContext, play.Target!, play.Card, -enemyStrength * 2);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}