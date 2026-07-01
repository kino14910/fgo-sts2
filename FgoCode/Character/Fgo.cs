using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Fgo.FgoCode.Cards;
using Fgo.FgoCode.Cards.DerivativeMash;
using Fgo.FgoCode.Extensions;
using Fgo.FgoCode.Relics;
using Fgo.FgoCode.Singletons;
using Fgo.FgoCode.UI;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Character;

public class Fgo : PlaceholderCharacterModel
{
    public const string CharacterId = "Fgo";

    public static readonly Color Color = new("f3ca49");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeFgo>(),
        ModelDb.Card<StrikeFgo>(),
        ModelDb.Card<StrikeFgo>(),
        ModelDb.Card<StrikeFgo>(),
        ModelDb.Card<DefendFgo>(),
        ModelDb.Card<DefendFgo>(),
        ModelDb.Card<DefendFgo>(),
        ModelDb.Card<DefendFgo>(),
        ModelDb.Card<WallOfSnowflakes>(),
        ModelDb.Card<DreamUponTheStars>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<SuitcaseFgo>(),
        ModelDb.Relic<BlessedLockOnChocolate>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<FgoCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<FgoRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<FgoPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();

    public override NCreatureVisuals? CreateCustomVisuals()
    {
        var placeholderPath = "np_button.png".CharacterUiPath();
        var visuals = NodeFactory<NCreatureVisuals>.CreateFromResource(placeholderPath);

        FgoCombatUi.AttachTo(visuals);

        return visuals;
    }

    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        this.FgoRes().AddStars();
        return base.AfterDamageGiven(choiceContext, dealer, result, props, target, cardSource);
    }
}