using BaseLib.Utils;
using Fgo.FgoCode.Singletons;
using Fgo.FgoCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class ExtremityOfVillains : FgoCard
{
    public ExtremityOfVillains() : base(1, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithTags(FgoEnums.Foreigner);
        WithDamage(8, 4);
        WithCards(2, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_heavy")
            .Execute(choiceContext);
        if (this.FgoRes().CanCrit)
        {
            await PlayerCmd.GainEnergy(1m, Owner);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }
}