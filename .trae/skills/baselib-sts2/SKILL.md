---
name: BaseLib-StS2
description: - Creating or reviewing any StS2 mod code (cards, relics, powers, events, characters)
- Implementing custom mechanics using BaseLib hooks and utilities
- Debugging localization, model registration, or pool issues
- Designing card descriptions with dynamic variables and SimpleLoc
---

# BaseLib-StS2 Framework Expert

## Description
Self-contained knowledge base for Slay the Spire 2 BaseLib mod development. Provides complete API reference, coding conventions, and architectural patterns derived from the BaseLib Wiki documentation. Source code supplementation available from `D:\projects\BaseLib-StS2`.

## When To Use
- Creating or reviewing any StS2 mod code (cards, relics, powers, events, characters)
- Implementing custom mechanics using BaseLib hooks and utilities
- Debugging localization, model registration, or pool issues
- Designing card descriptions with dynamic variables and SimpleLoc

---

## 1. Architecture Overview

### Model System
BaseLib uses a **Model-Node** architecture. Models define behavior; Nodes handle visuals/input.
- `CardModel` → `NCard` (visuals)
- `RelicModel` → `NRelic`
- `PowerModel` → `NPower`
- All models tracked in `ModelDb` (auto-loaded at launch)

### ID Prefixing
Any class inheriting `ICustomModel` (or any `CustomModel` class) automatically gets its **root namespace** prepended as an ID prefix. This prevents cross-mod ID conflicts.

### Pool Annotation
Content must belong to a pool. Use `[Pool(typeof(PoolType))]` on the class. The annotation is **inheritable**.
```csharp
[Pool(typeof(FgoCardPool))]
public abstract class FgoCard : CustomCardModel(...) { }
```

---

## 2. Cards (CustomCardModel)

### Constructor
```csharp
CustomCardModel(int cost, CardType type, CardRarity rarity, TargetType target)
```

### Key Overrides
| Override | Type | Purpose |
|----------|------|---------|
| `CanonicalTags` | `HashSet<CardTag>` | Strike, Defend, Innate tags |
| `CanonicalKeywords` | `HashSet<CardKeyword>` | Exhaust, Ethereal, Retain, Unplayable, Sly, Innate |
| `CanonicalVars` | `IEnumerable<DynamicVar>` | Damage, Block, Power, calculated vars |
| `OnPlay(choiceContext, play)` | `async Task` | Card effect execution |
| `OnUpgrade()` | `void` | Upgrade logic (modify DynamicVars) |
| `IsPlayable` | `bool` | Conditional playability |
| `ShouldGlowGoldInternal` | `bool` | Gold glow condition |
| `CustomPortraitPath` | `string` | Full art image path |
| `PortraitPath` | `string` | Small card image path |

### CommonActions Utility
```csharp
// Simplified attack
await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
// Multi-target
await CommonActions.CardAttack(this, play).Execute(choiceContext);
```

### Base Game Command Patterns
```csharp
// Damage
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this).Targeting(play.Target!).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
// Multi-hit
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .WithHitCount(hits).FromCard(this).Targeting(play.Target!).Execute(choiceContext);
// All enemies
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this).TargetingAllOpponents(CombatState!).Execute(choiceContext);

// Block
await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);

// Heal
await CreatureCmd.Heal(Owner.Creature, 3m);

// Energy
await PlayerCmd.GainEnergy(1m, Owner);

// Draw
await CardPileCmd.Draw(choiceContext, 2m, Owner);

// HP Loss (unblockable)
await CreatureCmd.Damage(choiceContext, target, amount,
    ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);

// Lose Max HP
await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, 2m, isFromCard: true);

// Lose Block (from enemy)
await CreatureCmd.LoseBlock(enemyCreature, 999m);
```

### Power Application
```csharp
// Apply to self
await PowerCmd.Apply<StrengthPower>(Owner.Creature, 2m, Owner.Creature, this);
// Apply to target
await PowerCmd.Apply<VulnerablePower>(play.Target!, 2m, Owner.Creature, this);
// Apply to all enemies
await PowerCmd.Apply<WeakPower>(CombatState!.HittableEnemies, 2m, Owner.Creature, this);
// Remove power
await PowerCmd.Remove(somePower);
// Check power amount
int strAmount = creature.GetPowerAmount<StrengthPower>();
bool hasPower = creature.HasPower<StrengthPower>();
```

### ConstructedCardModel (Alternative)
```csharp
public class FancyStrike : CustomCardModel(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    public FancyStrike() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(77, 23);
        WithTags(CardTag.Strike);
    }
}
```
Advantages: auto-generates tooltips for powers, shorter definitions.

---

## 3. Relics (CustomRelicModel)

```csharp
[Pool(typeof(FgoRelicPool))]
public abstract class FgoRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}
```
Override `GetUpgradeReplacement()` for starter relic upgrades.

---

## 4. Powers (PowerModel / CustomPowerModel)

### Key Overrides (from AbstractModel)
```csharp
// Damage modification
public virtual decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource);
public virtual decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource);

// Block modification
public virtual decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay);
public virtual decimal BlockMultiplicative(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay);

// Combat hooks
public virtual Task AfterAttack(AttackCommand command);
public virtual Task AfterDamageGiven(PlayerChoiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource);
public virtual Task BeforeDamageReceived(PlayerChoiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource);
public virtual Task AfterDamageReceived(PlayerChoiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource);
public virtual Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource);
public virtual Task AfterBlockBroken(Creature creature);

// Turn hooks
public virtual Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext);
public virtual Task AfterEnemyTurnStart(PlayerChoiceContext choiceContext);
public virtual Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side);
```

### Power Properties
```csharp
public override PowerType Type => PowerType.Buff;  // or Debuff
public override PowerStackType StackType => PowerStackType.Counter;  // Counter, Single, Duration, None
public override bool AllowNegative => false;
```

### StrengthPower Pattern (Damage Modifier)
```csharp
public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
{
    if (Owner != dealer) return 0m;
    if (!props.IsPoweredAttack()) return 0m;
    return Amount;
}
```

### WeakPower Pattern (Damage Reducer)
```csharp
public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
{
    if (Owner != dealer) return 1m;
    if (!props.IsPoweredAttack()) return 1m;
    return 0.75m;
}
```

### Turn-Based Removal Pattern
```csharp
public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
{
    if (side == CombatSide.Player)
        await PowerCmd.TickDownDuration(this);
    // or: await PowerCmd.Remove(this);
}
```

### Localization Required
Powers need localization in `powers.json`:
```json
{
  "MODID-POWER_CLASS_NAME.title": "Display Name",
  "MODID-POWER_CLASS_NAME.description": "Description with {0} for amount.",
  "MODID-POWER_CLASS_NAME.smartDescription": "Smart description."
}
```

---

## 5. Singletons (CustomSingletonModel)

Passive hook receivers. Constructor determines scope:
```csharp
public class MySingleton() : CustomSingletonModel(HookType.Combat)
{
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Global passive effect
    }
}
```

---

## 6. Events (CustomEventModel)

### Structure
Events use a page-based flow. Override `GenerateInitialOptions()` to define choices; option handlers are `async Task` methods.

### Key Overrides
| Override | Type | Purpose |
|----------|------|---------|
| `Acts` | `ActModel[]` | Acts to spawn in; empty = shared |
| `IsAllowed` | `bool` | Spawn condition based on run state |
| `IsShared` | `bool` | All players must choose same option |
| `GenerateInitialOptions()` | `IReadOnlyList<EventOption>` | Define choices |
| `CanonicalVars` | `IEnumerable<DynamicVar>` | Dynamic values (gold, HP, etc.) |
| `CalculateVars()` | `void` | Randomize vars per event instance |
| `Localization` | `List<(string, string)>` | In-code localization |
| `CustomInitialPortraitPath` | `string` | Portrait image |
| `CustomBackgroundScenePath` | `string` | Background scene |

### Localization Records
```csharp
EventLoc(title, params EventPageLoc[])
EventPageLoc(pageKey, description, params EventOptionLoc[])
EventOptionLoc(optionKey, title, description)
```
Keys generated: `{Entry}.pages.{page}.description`, `{Entry}.pages.{page}.options.{option}.title/description`

### Option Helpers
```csharp
Option(handler)                      // key derived from method name
Option(handler, HoverTipFactory...)  // with hover tips
LockedOption("LOCKED_KEY")           // disabled option
```

### Page Transitions
```csharp
SetEventFinished(PageDescription("PAGE_KEY"))   // end event
SetEventState(PageDescription("PAGE"), [options]) // switch to new page with options
```

### Commands in Events
Use `new ThrowingPlayerChoiceContext()` when a command requires a `PlayerChoiceContext`. Common commands: `PlayerCmd.GainGold`, `PlayerCmd.LoseGold`, `CreatureCmd.Heal`, `CreatureCmd.Damage`, `CreatureCmd.GainMaxHp`, `CreatureCmd.LoseMaxHp`, `RelicCmd.Obtain`, `CardPileCmd.Add`, `CardPileCmd.AddCurseToDeck<T>`.

### Example
```csharp
public class TestEvent() : CustomEventModel
{
    public override List<(string, string)> Localization => new EventLoc("Test",
        new EventPageLoc("INITIAL", "Initial page.",
            new EventOptionLoc("PLAIN", "This", "damage for gold"),
            new EventOptionLoc("ORNATE", "That", "Clumsy for relic")),
        new EventPageLoc("PLAIN", "You picked the boring option."),
        new EventPageLoc("ORNATE", "You picked the wrong option."));

    protected override IReadOnlyList<EventOption> GenerateInitialOptions() =>
    [
        Option(Plain).ThatDoesDamage(DynamicVars.HpLoss.IntValue),
        Option(Ornate, HoverTipFactory.FromCardWithCardHoverTips<Clumsy>())
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(6),
        new GoldVar(0),
        new StringVar("Curse", ModelDb.Card<Clumsy>().Title)
    ];

    public override void CalculateVars()
    {
        DynamicVars.Gold.BaseValue = Rng.NextInt(41, 69);
    }

    public async Task Plain()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner!.Creature,
            DynamicVars.HpLoss.IntValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);
        SetEventFinished(PageDescription("PLAIN"));
    }

    public async Task Ornate()
    {
        await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(Owner!).ToMutable(), Owner!);
        await CardPileCmd.AddCurseToDeck<Clumsy>(Owner!);
        SetEventFinished(PageDescription("ORNATE"));
    }

    public override string CustomInitialPortraitPath =>
        ImageHelper.GetImagePath($"events/{ModelDb.Event<ThisOrThat>().Id.Entry.ToLowerInvariant()}.png");
    public override string CustomBackgroundScenePath =>
        SceneHelper.GetScenePath("events/background_scenes/" + ModelDb.Event<ThisOrThat>().Id.Entry.ToLowerInvariant());
}
```

---

## 7. Temporary Powers (CustomTemporaryPowerModelWrapper)

```csharp
public class TempStrengthPower : CustomTemporaryPowerModelWrapper<MyCard, StrengthPower> {}
public class TempNegativePower : CustomTemporaryPowerModelWrapper<MyCard, StrengthPower>
{
    protected override bool InvertInternalPowerAmount => true;
    public override PowerType Type => PowerType.Debuff;
}
```
Properties: `LastForXExtraTurns`, `UntilEndOfOtherSideTurn`.

---

## 8. Dynamic Variables

### Standard Types
| Class | Constructor | Accessor |
|-------|-------------|----------|
| `DamageVar` | `new DamageVar(6m, ValueProp.Move)` | `DynamicVars.Damage` |
| `BlockVar` | `new BlockVar(6m, ValueProp.Move)` | `DynamicVars.Block` |
| `HealVar` | `new HealVar(6m)` | `DynamicVars.Heal` |
| `EnergyVar` | `new EnergyVar(1)` (int!) | `DynamicVars.Energy` |
| `CardsVar` | `new CardsVar(2)` (int!) | `DynamicVars.Cards` |
| `PowerVar<T>` | `new PowerVar<StrengthPower>(2m)` | `DynamicVars.Strength` or `DynamicVars["StrengthPower"]` |
| `IntVar` | `new IntVar("Name", 4m)` | `DynamicVars["Name"]` |

**IMPORTANT**: `EnergyVar` and `CardsVar` constructors take `int`, not `decimal`.

### Calculated Variables (BaseLib)
```csharp
// Quick define in cards
protected override IEnumerable<DynamicVar> CanonicalVars => [
    ..MakeCalculatedDamage(0, (card, target) => card.Owner.Creature.Block)
];
// Custom named
new CustomCalculatedVar("MyVar", ...) // looks for "MyVarExtra" and "MyVarBase"
```

### DisplayVar (arbitrary text)
```csharp
new DisplayVar<CardModel>("key", (model) => model.GetDynamicVar("Something").IntValue.ToString())
```

### DynamicVar Tooltips
```csharp
new DynamicVar("Name", 6m).WithTooltip(); // Localization: MODNAME-NAME
```

---

## 9. Localization System

### File Structure
```
mod_root/localization/
  eng/  (English)
    cards.json
    powers.json
    relics.json
    characters.json
    static_hover_tips.json
    card_keywords.json
  zhs/  (Chinese Simplified)
    ...
```

### Card Localization Keys
Format: `MODPREFIX-CARD_CLASS_NAME.title` and `.description`
- Class name is split on camelCase boundaries and uppercased
- `WildRule` → `WILD_RULE` → `FGO-WILD_RULE.title`
- `CharismaofAdversity` → `CHARISMAOF_ADVERSITY` → `FGO-CHARISMAOF_ADVERSITY.title`
- `NffSpecial` → `N_FF_SPECIAL` → `FGO-N_FF_SPECIAL.title`

### Description Variables
- `{Damage:diff()}` — shows damage with upgrade diff
- `{Block:diff()}` — block with diff
- `{Strength:diff()}` — PowerVar accessor
- `{CalculatedHits}` — calculated var
- `{MyIntVar}` — custom IntVar by name

### Simplified Localization (SimpleLoc)
Start description with `#` to enable:
- `!Var!` → `{Var:diff()}`
- `@Var@` → `{Var:inverseDiff()}`
- Short names: D=Damage, CD=CalculatedDamage, B=Block, CB=CalculatedBlock, C=Cards, E=Energy, H=Heal
- `*Word` → `[gold]Word[/gold]`
- `(plural)` → plural check
- `-text-` removed on upgrade, `+text+` added on upgrade
- `[EEE]` → fixed energy icons, `[E?]` → energy icons from variable

### Keyword Localization
In `card_keywords.json`:
```json
{ "MODPREFIX-KEYWORD_NAME.title": "...", "MODPREFIX-KEYWORD_NAME.description": "..." }
```

---

## 10. Custom Enums & Keywords

```csharp
[CustomEnum]
public static CardKeyword MyKeyword;

// Auto-add to card text (like Exhaust)
[CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
public static CardKeyword MyKeyword;

// Custom ID
[CustomEnum("CustomId")]
public static CardKeyword MyKeyword;
```
Must be `public static` (not readonly). Localization: `MODPREFIX-KEYWORD_NAME`.

---

## 11. SpireField

```csharp
// Basic
public static readonly SpireField<CardModel, int> SpecialNumber = new(() => 0);
int val = SpecialNumber[model];
SpecialNumber[model] = 5;

// Persistent (saves across sessions)
public static readonly SavedSpireField<Player, int> NpGauge = new(() => 0, "fgo_np_gauge");
```
Common: `SpireField<PlayerCombatState, ?>` for per-combat data.

---

## 12. Mod Configuration

```csharp
internal class MyConfig : SimpleModConfig
{
    public static bool Option { get; set; } = true;
    [ConfigSlider(0, 100, 5)]
    public static int Value { get; set; } = 50;
}
// In Initialize():
ModConfigRegistry.Register(ModId, new MyConfig());
```

---

## 13. Mod Interop

```csharp
[ModInterop("OtherModId", "OtherMod.Namespace")]
public static class Interop
{
    public static void Method(int num) { } // Mimics OtherMod method
    public static int Property { get; set; } // Accesses field/property
}
```

---

## 14. Targeting Types

Custom target types in `CustomTargetType`:
`Everyone`, `Anyone`, `AllAttackingEnemies`, `AnyAttackingEnemy`, `AllBlockingEnemies`, `AnyBlockingEnemy`, `AllNonBlockingEnemies`, `AnyNonBlockingEnemy`, `AllHighestHpEnemies`, `AllLowestHpEnemies`, `AnyFullLifeEnemy`, `AllFullLifeEnemies`

Multi-target: `this.GetTargets()` extension method.

---

## 15. Code Conventions

### Card File Structure
```csharp
using BaseLib.Utils;           // CommonActions
using Fgo.FgoCode.Cards;       // FgoCard base
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;  // Creature type
using MegaCrit.Sts2.Core.Entities.Powers;     // PowerType enum
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;              // CardModel
using MegaCrit.Sts2.Core.Models.Cards;        // CardKeyword
using MegaCrit.Sts2.Core.Models.Powers;       // StrengthPower etc.
using MegaCrit.Sts2.Core.ValueProps;

namespace Fgo.FgoCode.Cards;

public class MyCard() : FgoCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override HashSet<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade() { DynamicVars.Damage.UpgradeValueBy(3m); }
}
```

### Null Safety
- `play.Target` is `Creature?` — use `play.Target!` when targeting is guaranteed
- `CombatState` is `CombatState?` — use `CombatState!`
- `Owner` is `Player` (non-nullable), `Owner.Creature` is `Creature`

### ValueProp Flags
- `ValueProp.Move` — standard card damage (affected by Strength)
- `ValueProp.Unpowered | ValueProp.Move` — not affected by Strength
- `ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move` — HP loss

### Common Namespaces
| Type | Namespace |
|------|-----------|
| `Creature` | `MegaCrit.Sts2.Core.Entities.Creatures` |
| `PowerType` | `MegaCrit.Sts2.Core.Entities.Powers` |
| `StrengthPower` | `MegaCrit.Sts2.Core.Models.Powers` |
| `CardKeyword` | `MegaCrit.Sts2.Core.Models.Cards` |
| `CardModel` | `MegaCrit.Sts2.Core.Models` |
| `CommonActions` | `BaseLib.Utils` |
| `DynamicVar` | `MegaCrit.Sts2.Core.Localization.DynamicVars` |

### Available Powers (MegaCrit.Sts2.Core.Models.Powers)
StrengthPower, DexterityPower, VulnerablePower, WeakPower, FrailPower, PoisonPower, ThornsPower, RegenPower, ArtifactPower, IntangiblePower, VigorPower, PlatingPower, NoDrawPower, BufferPower, BlurPower, BarricadePower, CorruptionPower, DemonFormPower, EchoFormPower, etc.

### Source Code Reference
For precise API verification, consult: `D:\projects\BaseLib-StS2\` (BaseLib source) and `D:\sts-fgoMod\Slay the Spire 2\src\Core\` (game source).

---

## 16. CommonActions API (BaseLib.Utils)

Shortcut utility class for common card actions. Handles targeting types automatically.

### CardAttack (auto-targeting)
```csharp
// Simplest - auto-detects DamageVar/CalculatedDamageVar, auto-targets based on TargetType
await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
await CommonActions.CardAttack(this, play, hitCount: 3, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);

// Multi-target (handles custom TargetType automatically)
await CommonActions.CardAttack(this, play).Execute(choiceContext);
```

### CardBlock
```csharp
await CommonActions.CardBlock(this, play);  // auto-detects BlockVar/CalculatedBlockVar
```

### Draw
```csharp
await CommonActions.Draw(this, choiceContext);  // uses card's CardsVar
```

### Apply Power
```csharp
await CommonActions.Apply<StrengthPower>(target, this);           // apply using card's PowerVar
await CommonActions.Apply<StrengthPower>(target, this, 5m);      // apply specific amount
await CommonActions.ApplySelf<StrengthPower>(this);               // apply to self
await CommonActions.Apply<StrengthPower>(targets, this);          // multi-target
```

### Select Cards
```csharp
var selected = await CommonActions.SelectCards(choiceContext, this, cards, count);
var single = await CommonActions.SelectSingleCard(choiceContext, this, cards);
```

### Generate Cards
```csharp
var cards = await CommonActions.GenerateCards(this, count);
var card = await CommonActions.GenerateSingleCard(this);
```

---

## 17. Common-Commands-Cookbook (StS2 Patterns)

### Attack Commands

**Single Target** (`TargetType.AnyEnemy`):
```csharp
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this).Targeting(play.Target)
    .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
```

**All Enemies** (`TargetType.Self` + `TargetingAllOpponents`):
```csharp
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this).TargetingAllOpponents(CombatState!)
    .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3").Execute(choiceContext);
```

**Random Enemy**:
```csharp
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this).TargetingRandomOpponents(CombatState!)
    .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
```

**Multi-hit** (use `RepeatVar` or `IntVar`):
```csharp
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this).WithHitCount(DynamicVars.Repeat.IntValue)
    .Targeting(play.Target!).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
```

### Block Command
```csharp
await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
```

### Power Commands
```csharp
// Apply
await PowerCmd.Apply<StrengthPower>(target, amount, applier, cardSource);
await PowerCmd.Apply<StrengthPower>(targets, amount, applier, cardSource);  // multi-target

// Modify
await PowerCmd.Decrement(power);
await PowerCmd.Remove(power);
await PowerCmd.ModifyAmount(power, offset, applier, cardSource);
```

### Card Commands
```csharp
CardCmd.Upgrade(card);
await CardCmd.AutoPlay(choiceContext, card, target);
await CardCmd.Exhaust(choiceContext, card);
await CardCmd.Discard(choiceContext, card);
CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
CardCmd.Enchant<Sharp>(card, amount);
```

### Card Select Commands
```csharp
// From hand
var selected = await CardSelectCmd.FromHand(choiceContext, Owner, prefs, filter, cardSource);

// From grid (discard pile, etc.)
var cards = await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, Owner, prefs);
```

### CardPile Commands
```csharp
await CardPileCmd.Draw(choiceContext, count, Owner);
await CardPileCmd.Add(card, PileType.Hand);
await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Random, this);
await CardPileCmd.AddToCombatAndPreview<CardType>(Owner.Creature, PileType.Hand, count, Owner);
await CardPileCmd.AddCursesToDeck(cards, Owner);
```

### Heal & Energy
```csharp
await CreatureCmd.Heal(Owner.Creature, amount);
await CreatureCmd.Damage(choiceContext, target, amount, ValueProp.Unblockable | ValueProp.Unpowered, cardSource);
await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, amount, isFromCard: true);
await CreatureCmd.LoseBlock(target, amount);
await PlayerCmd.GainEnergy(amount, Owner);
await PlayerCmd.LoseEnergy(amount, Owner);
```

---

## 18. ConstructedCardModel (Alternative Card Builder)

Fluent builder API for cards. Constructor-based instead of property overrides.

```csharp
[Pool(typeof(FgoCardPool))]
public class FancyStrike : CustomCardModel(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    public FancyStrike() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(77, 23);           // base, upgrade
        WithBlock(10, 5);
        WithCards(2, 1);
        WithEnergy(1);
        WithHeal(6, 2);
        WithPower<StrengthPower>(2, 1);
        WithTags(CardTag.Strike);
        WithKeywords(CardKeyword.Exhaust);
        WithCostUpgradeBy(-1);        // reduce cost on upgrade
        WithTip(typeof(SomePower));   // auto-generate tooltip
    }
}
```

---

## 19. CustomTargetType Extended Targeting

```csharp
// Multi-target types (use with CombatState!.HittableEnemies or this.GetTargets())
CustomTargetType.Everyone
CustomTargetType.AllAttackingEnemies
CustomTargetType.AllBlockingEnemies
CustomTargetType.AllNonBlockingEnemies
CustomTargetType.AllHighestHpEnemies
CustomTargetType.AllLowestHpEnemies
CustomTargetType.AllFullLifeEnemies

// Single-target types (pass target to play.Target)
CustomTargetType.Anyone
CustomTargetType.AnyAttackingEnemy
CustomTargetType.AnyBlockingEnemy
CustomTargetType.AnyNonBlockingEnemy
CustomTargetType.AnyFullLifeEnemy

// Usage in card:
public class MyCard() : FgoCard(1, CardType.Attack, CardRarity.Common, CustomTargetType.AnyAttackingEnemy)

// Multi-target attack with filter:
await CommonActions.CardAttack(this, play).Execute(choiceContext);
```

---

## 20. BaseLib Hooks

### IHealAmountModifier
```csharp
public class MyHealModifier : CustomSingletonModel(HookType.Combat), IHealAmountModifier
{
    public decimal ModifyHealAdditive(Creature target, decimal amount) => 0m;
    public decimal ModifyHealMultiplicative(Creature target, decimal amount) => 1m;
}
```

### IMaxHandSizeModifier
```csharp
public class MyHandSizeModifier : CustomSingletonModel(HookType.Combat), IMaxHandSizeModifier
{
    public int ModifyMaxHandSize(Player player, int current) => current;
    public int ModifyMaxHandSizeLate(Player player, int current) => current + 2;
}
```

### IHealthBarForecastSource
Powers can directly implement this to show forecast on health bars. Other implementations register with `HealthBarForecastRegistry`.

---

## 21. BaseLib Keywords & Dynamic Variables

### Built-in Keywords
| Keyword | Effect |
|---------|--------|
| `BaseLibKeywords.Purge` | Card removed from deck when played |

### Built-in Dynamic Variables
| Variable | Card Text | Description |
|----------|-----------|-------------|
| Persist | `[gold]Persist[/gold] {Persist:diff()}.` | Returns to hand N times per turn |
| Exhaustive | `[gold]Exhaustive[/gold] {Exhaustive:diff()}.` | Exhausts after N uses per combat |
| Refund | `[gold]Refund[/gold] {Refund:diff()}.` | Returns energy cost after play |

### DynamicVar Tooltips
```csharp
new DynamicVar("Name", 6m).WithTooltip();  // Localization: MODNAME-NAME
new DynamicVar("Name", 6m).WithTooltip("CustomKey");  // Localization: CustomKey
```

### DisplayVar (arbitrary text in descriptions)
```csharp
new DisplayVar<CardModel>("key", (model) => model.GetDynamicVar("Something").IntValue.ToString())
```

### Quick Calculated Vars
```csharp
// In CanonicalVars:
..MakeCalculatedDamage(0, (card, target) => card.Owner.Creature.Block)
..MakeCalculatedBlock(0, (card, target) => card.Owner.Creature.MaxHp)
..MakeCalculatedVar("MyVar", baseValue, multiplier: 2, (card, target) => someValue)
```