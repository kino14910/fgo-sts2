using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Fgo.FgoCode.Cards.Colorless;

public class EightKindness : FgoColorlessCard
{
    public EightKindness() : base(3, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithPower<StrengthPower>(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var self = Owner.Creature;
        var amount = DynamicVars.Strength.BaseValue;

        await CommonActions.ApplySelf<StrengthPower>(choiceContext, play.Card);
        await CommonActions.Apply<DexterityPower>(choiceContext, self, play.Card, amount);
        await CommonActions.Apply<PlatingPower>(choiceContext, self, play.Card, amount);
        await CommonActions.Apply<RegenPower>(choiceContext, self, play.Card, amount);
        await CommonActions.Apply<ThornsPower>(choiceContext, self, play.Card, amount);
        await CommonActions.Apply<VigorPower>(choiceContext, self, play.Card, amount);
        await CommonActions.Apply<IntangiblePower>(choiceContext, self, play.Card, amount);
        await CommonActions.Apply<ArtifactPower>(choiceContext, self, play.Card, amount);
    }
}