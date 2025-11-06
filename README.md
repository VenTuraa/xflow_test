## XFlow Test — Architecture Overview

### 1) Project Structure (by Domain)
All gameplay code is grouped under `Assets/Scripts` by domain. This keeps features independent, clarifies ownership, and reduces cross‑feature coupling.

- `Core/`
  - Cross‑cutting abstractions, base types, utilities, configuration, and shared services used by multiple domains.
  - Examples: base `Controller` patterns, value objects, serialization helpers, time/clock abstraction, logging helpers.

- `Shop/`
  - The shop domain: product catalog, bundles, pricing, validation, and purchase orchestration.
  - Central entry point: `ShopController` (singleton). It mediates the flow between catalog (`ShopInventory`), domain entities (`Bundle`), and external systems (analytics, rewards).

- `Rewards/`
  - Reward definitions and grant logic triggered after a successful purchase or other gameplay events.
  - Provides clear, side‑effecting operations (grant currency, items, boosts) with idempotency where possible.

Dependency direction should be inward: `Shop` and `Rewards` can depend on `Core`, but not vice versa. Peer domains avoid knowing each other’s internals; communication happens through narrow interfaces or events.

### 2) Responsibilities and Collaborations
- `ShopController`
  - Owns reference to `ShopInventory` (runtime catalog) and exposes querying APIs (e.g., list bundles).
  - Orchestrates the purchase flow: validation → delay/payment → domain action (`Bundle.Purchase()`) → post‑actions (e.g., rewards, analytics).
  - Provides a single surface for UI and other systems to trigger shop operations.

- `ShopInventory`
  - Acts as the read‑only source of truth for available bundles/products at runtime.
  - Should support lookup by identifiers and offer immutable or copy‑on‑read collections for safety.

- `Bundle`
  - Encapsulates what is being bought and how it applies its effect (`Purchase()`), delegating to `Rewards` or other domain services as needed.

- `Rewards`
  - Implements concrete reward application (grant items/currency, unlock content) and validation (e.g., limits, duplication rules).

### 3) Data Flow and Lifecycle
1. Initialization
   - A system (bootstrap or scene entry) constructs/loads `ShopInventory` and passes it into `ShopController.SetInventory(...)`.
2. Querying
   - UI fetches data via `ShopController.GetBundles()` (prefer read‑only shapes for lists/models).
3. Purchase
   - UI triggers `ShopController.PurchaseBundleAsync(...)`.
   - Controller validates inputs, performs timing or payment steps, and executes domain action.
4. Post‑purchase
   - Rewards are granted; analytics/telemetry events are emitted; UI is notified via callbacks/events.

### My Recommendations/Improvements
- I prefer to use UniTask for async operations (instead of IEnumerator) for better async/await composition, fewer allocations, and clearer error/cancellation handling.
- Introduce a DI solution (e.g., Zenject/Extenject or a lightweight custom container) to compose `ShopInventory`, `Rewards`, and services outside business logic, improving testability and extensibility.