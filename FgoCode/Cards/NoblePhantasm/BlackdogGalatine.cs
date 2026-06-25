using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class BlackdogGalatine : NobleCard
{
    public BlackdogGalatine() : base(3, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(16, 4);
        WithPower<RegenPower>(6, 3);
        WithVar("Energy", 2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_fire").Execute(choiceContext);
        // 获得再生
        await CommonActions.ApplySelf<RegenPower>(choiceContext, play.Card);
        // 获得 [E][E]
        await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
    }
}