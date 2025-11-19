##  Architecture Overview

### Domain Map (Folders = asmdef)
- `Core/` (asmdef `Core`)
  - Cross-domain abstractions and runtime services that know nothing about concrete gameplay domains.
  - Types: `PlayerData`, `IBundleCost`, `IBundleReward`, abstract `BundleCostReward` (SO base), resource view registries.

- `Shop/` (asmdef `Shop` -> references `Core` + TMP/UGUI)
  - Catalog and purchase orchestration. UI shell for list and details scenes.
  - Types: `ShopInventory` (SO), `Bundle` (SO), `ShopController`, `ShopSceneController`, `BundleDetailSceneController`, `ShopView`, `BundleCardView`.

- `Rewards/Gold` (asmdef `Gold` -> references `Core`)
  - Types: `GoldController`, `GoldData`, `GoldResourceView`, SO bricks: `FixedGoldCost`, `FixedGoldReward`.

- `Rewards/Health` (asmdef `Health` -> references `Core`)
  - Types: `HealthController`, `HealthData`, `HealthResourceView`, SO bricks: `FixedHealthCost`, `FixedHealthReward`, `PercentHealthCost`, `PercentHealthReward`.

- `Rewards/Location` (asmdef `Location` -> references `Core`)
  - Types: `LocationController`, `LocationData`, `LocationResourceView`, SO brick: `LocationReward`.

- `Rewards/VIP` (asmdef `VIP` -> references `Core`)
  - Types: `VIPController`, `VIPData`, `VIPResourceView`, SO bricks: `VIPReward`, `VIPCost`.

Dependency rule: `Gold`, `Health`, `Location`, `VIP`, `Shop` -> depend on `Core` only. There are no peer-to-peer dependencies.

### Core Layer
- `PlayerData`
  - Singleton runtime store keyed by type: `GetData<T>()/SetData<T>()`. No explicit fields for domain concepts (no `health`, `vip`, etc.). Each domain defines its own `*Data` class stored inside `PlayerData`.

- Shop abstractions
  - `IBundleCost` (can buy? apply cost), `IBundleReward` (apply reward), `BundleCostReward : ScriptableObject` as a neutral base so designers can compose lists inside a `Bundle` SO without `Shop` knowing concrete domains.

- UI resource registries
  - `ResourceViewInitializationRegistry` wires up resource views with an `onResourcesChanged` callback.
  - `ResourceViewUpdaterRegistry` allows global refresh of displayed resource values after changes.
  - `ResourceViewCallbackRegistry` collects callbacks so resource views can notify shop cards to re-evaluate affordability.

### Domain Implementations (Examples)
- Gold
  - `GoldData` holds `CurrentGold`.
  - `GoldController` modifies/queries gold and validates availability.
  - SO bricks: `FixedGoldCost : BundleCostReward, IBundleCost`, `FixedGoldReward : BundleCostReward, IBundleReward`.

- Health
  - `HealthData` holds `CurrentHealth`.
  - `HealthController` encapsulates add/remove/validate and edge-clamping.
  - SO bricks: `FixedHealthCost/Reward`, `PercentHealthCost/Reward` (percent from current value at the moment of apply).

- Location
  - `LocationData` holds a string `CurrentLocation`.
  - `LocationController` sets or resets to default; SO brick `LocationReward` switches location.

- VIP
  - `VIPData` holds `TimeSpan VIPDuration`.
  - `VIPController` add/remove/validate VIP time; SO bricks `VIPReward`, `VIPCost` operate with a composed duration.

Each domain contains:
- `*Data` (stored in `PlayerData`).
- `*Controller` (business logic; singleton for simplicity).
- Optional `*ResourceView` (UI for header with a `+` button behavior defined per domain).
- SO bricks implementing `IBundleCost` / `IBundleReward` and inheriting `BundleCostReward`.

### Shop Domain
- `ShopInventory` (SO): list of `Bundle` assets configured in the Editor.
- `Bundle` (SO):
  - `List<BundleCostReward> _costs`, `List<BundleCostReward> _rewards`.
  - `CanAfford()` - iterates costs; only checks those implementing `IBundleCost`.
  - `Purchase()` - applies costs then rewards atomically from the client perspective.
- `ShopController` (singleton):
  - Holds the active `ShopInventory` and exposes `GetBundles()`.
  - `PurchaseBundleAsync()` simulates a 3-second backend delay, then calls `bundle.Purchase()` and completion callback.
- Scene controllers and views:
  - `ShopSceneController` spawns bundle cards, wires callbacks for buy/info, and refreshes resource/UI after purchase.
  - `BundleDetailSceneController` shows a single card (without the info button) and supports purchase.
  - `BundleCardView` handles "Купить" button state, shows "Обработка..." during delay, and exposes an optional "i" button.
  - `ShopView` manages card instances and wires resource header views through registries.

### UI / MVC Orientation
- View components per domain inherit from `ResourceViewBase` and implement:
  - `GetDisplayText()` - how to render the current value.
  - `OnButtonClicked()` - domain-specific `+` behavior:
    - Gold: add +100.
    - Health: add +50.
    - Location: reset to default.
    - VIP: add +30 seconds; shown always in seconds.
- After changes, resource views call `_onResourceChanged` which triggers update callbacks, causing shop cards to re-evaluate affordability.

### Purchase Lifecycle
1) Player presses "Buy" on a card.
2) Card switches to processing state; button shows "Processing..." and becomes disabled.
3) `ShopController.PurchaseBundleAsync` waits 3s, then calls `Bundle.Purchase()`.
4) `Bundle` applies all costs then all rewards.
5) UI refresh via `ResourceViewUpdaterRegistry.RefreshAll()` and card state re-evaluation.

### Extensibility
- Add a new domain (e.g., Leaderboards):
  1) Create `Rewards/Leaderboards` folder + asmdef referencing `Core`.
  2) Define `LeaderboardData` and `LeaderboardController` using `PlayerData`.
  3) Implement SO bricks inheriting `BundleCostReward` and implementing `IBundleCost`/`IBundleReward` (e.g., `FixedLeaderboardPointsReward`).
  4) Optionally add a resource view for header display and `+` behavior.
  5) No changes needed in `Shop` (cards and bundle composition already work generically).

- Add a new brick inside an existing domain:
  - Create a new `ScriptableObject : BundleCostReward` implementing one of the interfaces.
  - Expose fields via `[SerializeField]` for designer control.
  - Reference the new asset in `Bundle` costs/rewards lists.

### Scenes and Setup
- Scenes:
  - `ShopScene` - list of bundles, resource header.
  - `BundleDetailScene` - full-screen single card with back button.
- Ensure both scenes are added to Build Settings.
- Create assets in Editor:
  - `Shop/SO/ShopInventory` - reference created `Shop/Bundle` assets.
  - `Shop/SO/Bundle` - fill `Costs`/`Rewards` lists with bricks from domain folders.

### My Recommendations/Improvements
- I prefer to use UniTask for async operations (instead of IEnumerator) for better async/await composition, fewer allocations, and clearer error/cancellation handling.
- Introduce a DI solution (e.g., Zenject/Extenject or a lightweight custom container) to compose `ShopInventory`, `Rewards`, and services outside business logic, improving testability and extensibility.
