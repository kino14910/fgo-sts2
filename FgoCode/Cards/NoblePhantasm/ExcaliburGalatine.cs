using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class ExcaliburGalatine : NobleCard
{
    public ExcaliburGalatine() : base(2, CardType.Attack,
        CardRarity.Rare, TargetType.Self)
    {
        WithDamage(24, 6);
        WithPower<VigorPower>(4);
        WithVar("SunlightTurns", 3);
        WithVar("CritDamage", 50);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActions.Apply<SunlightPower>(choiceContext, Owner.Creature, play.Card,
            DynamicVars["SunlightTurns"].BaseValue);
        await CommonActions.ApplySelf<VigorPower>(choiceContext, play.Card);
        if (Owner.Creature.HasPower<SunlightPower>())
            await CommonActions.Apply<CriticalDamagePower>(choiceContext, Owner.Creature, play.Card,
                DynamicVars["CritDamage"].BaseValue);
    }
}