using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class Sevendrive : NobleCard
{
    public Sevendrive() : base(3, CardType.Attack, TargetType.Self)
    {
        WithDamage(12, 4);
        WithVar("Strength", 2);
        WithNp(20);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // 获得2临时力量（回合结束自动失去）
        await CommonActions.ApplySelf<TemporaryStrengthPower>(choiceContext, play.Card,
            DynamicVars["Strength"].BaseValue);

        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);

        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}