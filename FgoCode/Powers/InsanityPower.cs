using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Powers;

public class InsanityPower : FgoPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "trigger_after_attacks_power.png".PowerImagePath();
    public override string CustomBigIconPath => "trigger_after_attacks_power.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Attack) return;
        var curse = Owner.GetPower<CursePower>();
        if (curse == null) return;

        Flash();
        await PowerCmd.Decrement(curse);
        await CommonActions.Apply<StrengthPower>(choiceContext, Owner, cardPlay.Card, 1m);
        await FgoNpCmd.AddNp(Amount);
    }
}