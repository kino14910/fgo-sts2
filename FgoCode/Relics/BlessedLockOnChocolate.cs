using Fgo.FgoCode.Cards.DerivativeMash;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;

namespace Fgo.FgoCode.Relics;

/// <summary>
///     祝福锁定巧克力：击败 Boss 后强化马修卡牌。
///     Level 0: Camelot + WallOfSnowflakes
///     Level 1 (Act1 Boss): LordCamelot + VeneratedWallOfSnowflakes + ObscurantWallofChalk
///     Level 2 (Act2 Boss): LordChaldeas + VeneratedShieldOfSnowflakes + ObscurantWallofChalk
/// </summary>
public class BlessedLockOnChocolate : FgoRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    /// <summary>
    ///     马修卡牌强化等级：0=初始, 1=第一层Boss后, 2=第二层Boss后
    /// </summary>
    public int MashUpgradeLevel { get; set; }

    public override async Task AfterCombatVictory(CombatRoom room)
    {
        if (room.RoomType != RoomType.Boss) return;
        if (MashUpgradeLevel >= 2) return;

        MashUpgradeLevel++;
        Flash();

        if (MashUpgradeLevel == 1)
            // 第一次升级：WallOfSnowflakes -> VeneratedWallOfSnowflakes
            foreach (var card in Owner.Deck.Cards.OfType<WallOfSnowflakes>().ToList())
                await CardCmd.Transform(card, ModelDb.Card<VeneratedWallOfSnowflakes>().ToMutable(),
                    CardPreviewStyle.None);
        else if (MashUpgradeLevel == 2)
            // 第二次升级：VeneratedWallOfSnowflakes -> VeneratedShieldOfSnowflakes
            foreach (var card in Owner.Deck.Cards.OfType<VeneratedWallOfSnowflakes>().ToList())
                await CardCmd.Transform(card, ModelDb.Card<VeneratedShieldOfSnowflakes>().ToMutable(),
                    CardPreviewStyle.None);
    }
}