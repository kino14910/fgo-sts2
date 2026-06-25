using BaseLib.Abstracts;
using Fgo.FgoCode.Extensions;
using Godot;

namespace Fgo.FgoCode.Character;

public class FgoPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Fgo.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}