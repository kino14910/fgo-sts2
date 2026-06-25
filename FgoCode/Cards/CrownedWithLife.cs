using BaseLib.Utils;
using Fgo.FgoCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Fgo.FgoCode.Cards;

public class CrownedWithLife : FgoCard
{
    public CrownedWithLife() : base(-2, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithVar("Tokens", 2, 1);
        WithPower<NonStackableGutsPower>(30, 10);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card is CrownedWithLife)
        {
            // 抽到时获得 Guts 效果和能量令牌，然后消耗
            await CommonActions.Apply<NonStackableGutsPower>(choiceContext, Owner.Creature, this,
                DynamicVars["NonStackableGutsPower"].BaseValue);
            await PlayerCmd.GainEnergy(DynamicVars["Tokens"].IntValue, Owner);
            await CardCmd.Exhaust(choiceContext, card, fromHandDraw);
        }
    }
}