using BaseLib.Audio;

namespace Fgo.FgoCode.Utils;

public record FgoSounds
{
    public static readonly ModSound UBW_Music = new(MusicPath("UBW_Extended.mp3"));
    public static readonly ModSound MasterChoose = new(SoundPath("MASTER_CHOOSE.mp3"));
    public static readonly ModSound Padoru = new(SoundPath("Padoru.mp3"));
    public static readonly ModSound MasterCurse = new(SoundPath("MASTER_CURSE.wav"));
    public static readonly ModSound MasterInvictusSpiritus = new(SoundPath("MASTER_INVICTUS_SPIRITUS.mp3"));

    public static readonly ModSound Gun = new(SoundPath("hermit_gun2.ogg"));
    public static readonly ModSound Kanshou = new(SoundPath("S011_Attack6.ogg"));
    public static readonly ModSound Sokoda = new(SoundPath("S011_Attack4.ogg"));
    public static readonly ModSound Segawaruku = new(SoundPath("S011_Skill1.ogg"));
    public static readonly ModSound TraceOn = new(SoundPath("S011_Skill2.ogg"));
    public static readonly ModSound Konosaida = new(SoundPath("S011_Skill3.ogg"));

    public static readonly ModSound UBW_Incantation = new(SoundPath("UBW.ogg"));

    private static string MusicPath(string file)
    {
        return $"res://Fgo/audio/music/{file}";
    }

    private static string SoundPath(string file)
    {
        return $"res://Fgo/audio/sound/{file}";
    }
}