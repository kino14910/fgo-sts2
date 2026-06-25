using BaseLib.Utils;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.Colorless;

public class InfiniteSuffering : FgoColorlessCard
{
    public InfiniteSuffering() : base(2, CardType.Attack,
        CardRarity.Token, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithDamage(8, 2);
        WithPower<VulnerablePower>(2, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<VulnerablePower>(choiceContext, enemy, play.Card);
        await FgoCardActions.AddToPile(ModelDb.Card<TheAbsoluteSword>().ToMutable(), PileType.Hand);
    }
}