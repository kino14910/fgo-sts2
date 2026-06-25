using BaseLib.Abstracts;
using Fgo.FgoCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Fgo.FgoCode.Character;

public class FgoCardPool : CustomCardPoolModel
{
    public override string Title => Fgo.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 1f; //Hue; changes the color. (f3ca49 gold)
    public override float S => 1f; //Saturation
    public override float V => 1f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load Fgo/images/cards/frame.png

        return card.Type switch
        {
            CardType.Attack => PreloadManager.Cache.GetTexture2D("cards/frame_attack.png".ImagePath()),
            CardType.Skill => PreloadManager.Cache.GetTexture2D("cards/frame_skill.png".ImagePath()),
            CardType.Power => PreloadManager.Cache.GetTexture2D("cards/frame_power.png".ImagePath()),
            _ => PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath()),
        };
    }

    //Color of small card icons
    public override Color DeckEntryCardColor => new("f3ca49");

    public override bool IsColorless => false;
}