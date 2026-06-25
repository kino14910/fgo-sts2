using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Fgo.FgoCode.Cards.Colorless.OptionCards;

public class ReleaseNoblePhantasm() : FgoColorlessCard(-2, CardType.Power, CardRarity.Token, TargetType.None)
{
    public override string PortraitPath => "command_spell_np.png".CardImagePath();
}