using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class KurKigalIrkalla : NobleCard
{
    public KurKigalIrkalla() : base(1, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(26, 8);
    }

    protected override bool ShouldGlowGoldInternal =>
        Owner.Creature.HasPower<BlessingOfKurPower>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
            await CreatureCmd.Damage(choiceContext, enemy,
                (int)(enemy.MaxHp / 10.0f), ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
                this);
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);

        var blessing = Owner.Creature.GetPower<BlessingOfKurPower>();
        if (blessing != null)
        {
            blessing.Flash();
            await CreatureCmd.Heal(Owner.Creature, blessing.Amount, false);
            await CommonActions.ApplySelf<StrengthPower>(choiceContext, play.Card, blessing.Amount / 3m);
            await PowerCmd.Remove(blessing);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m);
    }
}