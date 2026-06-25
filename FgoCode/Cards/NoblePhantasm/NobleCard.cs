using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Fgo.FgoCode.Character;
using Fgo.FgoCode.Extensions;
using Fgo.FgoCode.Singletons;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards.NoblePhantasm;

[Pool(typeof(NobleCardPool))]
public abstract class NobleCard : ConstructedCardModel
{
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigNobleCardImagePath();

    protected NobleCard(int cost, CardType type, CardRarity rarity, TargetType target)
        : base(cost, type, rarity, target)
    {
        WithKeywords(CardKeyword.Retain, CardKeyword.Exhaust);
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card is NobleCard)
            this.FgoRes().GainOverCharge(1);

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Generates an NP (Noble Phantasm) variable with given base value.
    /// </summary>
    protected NobleCard WithNp(int baseVal, int upgrade = 0)
    {
        WithVar("NP", baseVal, upgrade);
        return this;
    }

    /// <summary>
    ///     Generates a Star variable with given base value.
    /// </summary>
    protected NobleCard WithStar(int baseVal, int upgrade = 0)
    {
        WithVar("Star", baseVal, upgrade);
        return this;
    }
}