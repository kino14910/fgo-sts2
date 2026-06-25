using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.Colorless;

public class CurseDisaster : FgoColorlessCard
{
    public CurseDisaster() : base(-2, CardType.Status,
        CardRarity.Status, TargetType.Self)
    {
        WithKeywords(CardKeyword.Ethereal);
        WithPower<CursePower>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<CursePower>(choiceContext, play.Card);
        foreach (var enemy in CombatState!.HittableEnemies)
            await CommonActions.Apply<CursePower>(choiceContext, enemy, play.Card);
    }
}