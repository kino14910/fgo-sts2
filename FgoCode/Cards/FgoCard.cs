using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Fgo.FgoCode.Character;
using Fgo.FgoCode.Commands;
using Fgo.FgoCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Fgo.FgoCode.Cards;

[Pool(typeof(FgoCardPool))]
public abstract class FgoCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ConstructedCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await FgoNpCmd.AddNp(cardPlay.PlayCount, Owner);
    }

    /// <summary>
    ///     Generates an NP (Noble Phantasm) variable with given base value.
    /// </summary>
    protected FgoCard WithNp(int baseVal, int upgrade = 0)
    {
        WithVar("NP", baseVal, upgrade);
        return this;
    }

    /// <summary>
    ///     Generates a Star variable with given base value.
    /// </summary>
    protected FgoCard WithStar(int baseVal, int upgrade = 0)
    {
        WithVar("Star", baseVal, upgrade);
        return this;
    }
}