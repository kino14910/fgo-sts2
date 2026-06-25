using BaseLib.Utils;
using Fgo.FgoCode.Cards.NoblePhantasm;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.DerivativeMash;

/// <summary>
///     拟似展开／人理之础：马修初始宝具卡。
/// </summary>
public class Camelot : NobleCard
{
    public Camelot() : base(1, CardType.Power, TargetType.Self)
    {
        WithPower<PlatingPower>(1, 1);
        WithVar("DamageReduction", 20, 10);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<PlatingPower>(choiceContext, play.Card);
        await CommonActions.Apply<ReducePercentDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["DamageReduction"].BaseValue);
    }
}