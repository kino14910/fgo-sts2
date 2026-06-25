using BaseLib.Utils;
using Fgo.FgoCode.Cards.NoblePhantasm;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Fgo.FgoCode.Cards.DerivativeMash;

public class LordChaldeas : NobleCard
{
    public LordChaldeas() : base(1, CardType.Power,
        CardRarity.Status, TargetType.Self)
    {
        WithVar("DamageReduction", 30, 20);
        WithPower<PlatingPower>(5, 5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<ReducePercentDamagePower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["DamageReduction"].BaseValue);
        await CommonActions.ApplySelf<PlatingPower>(choiceContext, play.Card);
        await CommonActions.ApplySelf<ArtifactPower>(choiceContext, play.Card, 1m);
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, play.Card, 3m);
        await CommonActions.ApplySelf<NpDamagePower>(choiceContext, play.Card, 30m);
        await CommonActions.ApplySelf<ForcedNpCardPower>(choiceContext, play.Card, 1m);

        // 自身变为印证希望的人理之剑
        await CardCmd.Transform(this, ModelDb.Card<LordChaldeasAtlas>().ToMutable(), CardPreviewStyle.None);

        // 时为朦胧的白垩之壁变为测定时间的紫弹之薪
        var chalk = Owner.PlayerCombatState.Hand.Cards
            .FirstOrDefault(card => card is ObscurantWallofChalk);
        if (chalk != null)
            await CardCmd.Transform(chalk, ModelDb.Card<TimewornBulletKindling>().ToMutable(), CardPreviewStyle.None);
    }
}