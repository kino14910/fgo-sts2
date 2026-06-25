using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Fgo.FgoCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public abstract class FgoColorlessCard(int cost, CardType type, CardRarity rarity, TargetType target)
    : FgoCard(cost, type, rarity, target)
{
}