using BaseLib.Utils;
using Fgo.FgoCode.Cards.NoblePhantasm;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.DerivativeMash;

/// <summary>
///     印证希望的人理之剑：LordChaldeas 变身后的攻击形态。
/// </summary>
public class LordChaldeasAtlas : NobleCard
{
    public LordChaldeasAtlas() : base(1, CardType.Attack,
        CardRarity.Status, TargetType.AnyEnemy)
    {
        WithDamage(20, 10);
        WithPower<NpDamagePower>(30);
        WithNp(30, 20);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
        await CommonActions.ApplySelf<NpDamagePower>(choiceContext, play.Card);
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue);
    }
}