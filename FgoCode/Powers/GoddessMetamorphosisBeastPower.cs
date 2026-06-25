using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Powers;

public class GoddessMetamorphosisBeastPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "trigger_after_attacks_power.png".PowerImagePath();
    public override string CustomBigIconPath => "trigger_after_attacks_power.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Attack) return;
        if (cardPlay.Target == null) return;
        Flash();
        await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, Amount, Owner, null);
        await PowerCmd.Apply<CursePower>(choiceContext, cardPlay.Target, 1m, Owner, null);
    }
}