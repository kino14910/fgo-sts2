using BaseLib.Utils;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Powers;

/// <summary>
///     深渊之死：攻击时为敌人施加死亡概率。
/// </summary>
public class DeathOfDeathPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "trigger_after_attacks_power.png".PowerImagePath();
    public override string CustomBigIconPath => "trigger_after_attacks_power.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Attack) return;
        if (cardPlay.Target == null || cardPlay.Target == Owner) return;

        Flash();
        await CommonActions.Apply<DeathChancePower>(choiceContext, cardPlay.Target, cardPlay.Card, Amount);
    }
}