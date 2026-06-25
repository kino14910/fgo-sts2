using BaseLib.Config;

namespace Fgo.FgoCode;

[ConfigHoverTipsByDefault]
internal class FgoConfig : SimpleModConfig
{
    [ConfigSlider(0, 10, 1, Format = "{0:0}%")]
    public static int BaseNPPerCost { get; set; } = 5;

    public static bool EnableColorlessCards { get; set; } = true;
    public static bool EnablePadoru { get; set; } = false;
    public static bool EnableFtue { get; set; } = true;
    public static bool EnableNoCostNoblePhantasm { get; set; } = false;

    [ConfigSection("Enemies")]
    public static bool EnableEnemies { get; set; } = true;

    [ConfigVisibleIf(nameof(EnableEnemies))]
    public static bool EnableEmiya { get; set; } = true;
    [ConfigVisibleIf(nameof(EnableEnemies))]
    public static bool EnableCalamityOfNorwich { get; set; } = true;
    [ConfigVisibleIf(nameof(EnableEnemies))]
    public static bool EnableCernunnos { get; set; } = true;
    [ConfigVisibleIf(nameof(EnableEnemies))]
    public static bool EnableFaerieKnightGawain { get; set; } = true;
    [ConfigVisibleIf(nameof(EnableEnemies))]
    public static bool EnableFaerieKnightLancelot { get; set; } = true;
    [ConfigVisibleIf(nameof(EnableEnemies))]
    public static bool EnableMoss { get; set; } = true;
    [ConfigVisibleIf(nameof(EnableEnemies))]
    public static bool EnableQueenMorgan { get; set; } = true;
}
