using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

public class OriginBullet : FgoCard
{
    public OriginBullet() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<IgnoresInvincibilityPower>(choiceContext, play.Target!, play.Card, 1m);
    }
}