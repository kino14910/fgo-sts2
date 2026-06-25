using BaseLib.Abstracts;
using BaseLib.Utils;
using Fgo.FgoCode.Character;

namespace Fgo.FgoCode.Potions;

[Pool(typeof(FgoPotionPool))]
public abstract class FgoPotion : CustomPotionModel;