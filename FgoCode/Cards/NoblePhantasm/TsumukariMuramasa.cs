using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class TsumukariMuramasa : NobleCard
{
    public TsumukariMuramasa() : base(3, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(6, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, 8, "vfx/vfx_attack_slash").Execute(choiceContext);

        await PowerCmd.Remove<StrengthPower>(Owner.Creature);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}