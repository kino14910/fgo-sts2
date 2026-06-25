using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Powers;

public class CursePower : FgoPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override string CustomPackedIconPath => "every_turn_debuff_power.png".PowerImagePath();
    public override string CustomBigIconPath => "every_turn_debuff_power.png".BigPowerImagePath();
    public override bool AllowNegative => false;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        await CreatureCmd.Damage(choiceContext, Owner, Amount,
            ValueProp.Unblockable | ValueProp.Unpowered, null, null);
    }
}