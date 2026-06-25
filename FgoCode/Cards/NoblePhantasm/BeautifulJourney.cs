using BaseLib.Utils;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

public class BeautifulJourney : NobleCard
{
    public BeautifulJourney() : base(2, CardType.Attack, TargetType.Self)
    {
        WithDamage(24, 6);
        WithNp(20);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<NpRatePower>(choiceContext, play.Card, 1m);
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await FgoNpCmd.AddNp(DynamicVars["NP"].IntValue * CombatState!.HittableEnemies.Count());
    }
}