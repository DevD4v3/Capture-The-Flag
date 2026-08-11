# Architectural Decisions

This section documents architectural decisions that have a meaningful impact on the maintainability, portability, and testability of the game mode.

These decisions capture the reasoning behind important design choices and the context in which they were made. They are intended to preserve knowledge that may not be apparent from the implementation alone.

Architecture is treated as an evolving part of the software rather than a decision made only before implementation. As the game mode evolves, new constraints and requirements can lead to new architectural decisions or changes to existing ones.

The goal is not to follow architectural principles rigidly, but to make deliberate decisions that support the needs of the game mode while keeping its design understandable, maintainable, and testable.

## Index

- [Migration to SampSharp.Entities](#migration-to-sampsharpentities)
- [Domain Decisions and Side Effects](#domain-decisions-and-side-effects)
- [Encapsulating State-Related Side Effects](#encapsulating-state-related-side-effects)
- [Map Object Definitions](#map-object-definitions)
- [Database Independence](#database-independence)
- [Domain-Oriented Structure](#domain-oriented-structure)

## Migration to SampSharp.Entities

From v1.x through v8.x, the game mode was built using `SampSharp.GameMode`, the traditional SampSharp framework for developing SA-MP gamemodes written in C# using an object-oriented approach.

The main reason for migrating was the long-term evolution of the game mode. As more gameplay features were introduced, the `Player` entity became increasingly coupled to the state required by those features. Since each connected player needs to maintain their own state on the server, this state progressively accumulated around the `Player` entity.

Even when responsibilities were separated into different classes or services, the state of those features still had to be associated with the corresponding `Player`. The entity therefore remained the central point for storing and accessing player-specific state.

The game mode was therefore reimplemented for v9.x using `SampSharp.Entities`, the SampSharp framework for building gamemodes using an [Entity Component System (ECS)](https://en.wikipedia.org/wiki/Entity_component_system) pattern. The goal was to decouple feature-specific state from the `Player` entity itself, allowing features to attach their own state to player entities through independent components without requiring the entity to know about all the state associated with the player.

The main trade-off of the migration was the risk of introducing new bugs while rewriting the game mode. However, the long-term benefits of decoupling player-specific state from the `Player` entity were considered significant enough to justify the migration.

## Domain Decisions and Side Effects

A domain decision represents a rule or condition that determines what is allowed to happen in the game. For example, a flag can only be captured when a player picks up the opposing team's flag while it is at its base position.

Domain decisions should remain separate from the side effects that occur as a consequence of those decisions. Keeping these responsibilities separate reduces coupling between domain logic and the mechanisms used to react to its decisions, while also making the domain logic easier to test independently.

When a player interacts with a team's flag, `FlagSystem` delegates the interaction to the domain model. `Team.HandleFlagInteraction(player)` evaluates the relevant domain rules and determines the resulting state transition, returning a `FlagStatus`. 
`FlagSystem` then dispatches the corresponding event handler based on that flag state.

This keeps the domain model responsible for domain decisions, while event handlers are responsible for reacting to the result and performing side effects such as updating player TextDraws, playing sounds, spawning pickups, granting rewards, or persisting player statistics.

```text
open.mp
  │
  │  1. Fires event
  ▼
OnPlayerPickUpPickup
  │
  │  2. Handled by
  ▼
FlagSystem
  │
  │  3. Ask the domain to handle the interaction
  ▼
Team.HandleFlagInteraction(player)
  │
  ├── Flag.Capture()
  ├── Flag.ReturnToBase()
  ├── Flag.Take()
  └── Flag.Drop()
  │
  │  4. Returns the resulting domain state
  ▼
FlagStatus
  (BasePosition, Captured, Returned, Taken, ...)
  │
  ▼
FlagSystem
  │
  │  Receives the resulting flag state
  │
  │  5. Dispatch corresponding handler
  ▼
IFlagEvent
  │
  ├── OnFlagCaptured ──────┐
  ├── OnFlagReturned ──────┤
  ├── OnFlagDropped ───────┤
  ├── OnFlagScore ─────────┤
  ├── OnFlagTaken ─────────┤
  └── OnFlagAtBasePosition ┤
                           │
                           ▼
                    Side Effects
                           │
                           ├── Update player TextDraws
                           ├── Play sounds
                           ├── Spawn pickups
                           ├── Grant rewards
                           └── Persist player statistics
```

Not every domain operation needs to produce a state transition. When an operation is better represented by a discrete result, the domain can return that result directly. The result can then be handled independently, allowing the domain logic to remain decoupled from its side effects.

In `GunGame`, `GunGame.ProcessKill()` applies the GunGame rules to a kill and returns a `GunGameResult`. The result represents what happened as a consequence of processing the kill, such as leveling up, leveling down, reaching the final level, scoring the final kill, or taking no progression-related action.

`GunGameSystem` then uses the `GunGameResult` to dispatch the corresponding `IGunGameResultHandler`. The handlers are responsible for reacting to the result and performing the required side effects, while the `GunGame` domain object remains independent of those effects.

```text
open.mp
  │
  │  1. Fires event
  ▼
OnPlayerDeath
  │
  │  2. Handled by
  ▼
GunGameSystem
  │
  │  3. Ask the domain to process the kill
  ▼
GunGame.ProcessKill(killer, victim, reason)
  │
  │  4. Returns the domain result
  ▼
GunGameResult
  (None, LeveledUp, LeveledDown,
   ReachedFinalLevel, ScoredFinalKill)
  │
  ▼
GunGameSystem
  │
  │   Receives the domain result
  │
  ├── 5. Dispatch corresponding handler
  │      │
  │      ▼
  │   IGunGameResultHandler
  │      │
  │      ├── PlayerLeveledUp
  │      ├── PlayerLeveledDown
  │      ├── PlayerReachedFinalLevel
  │      └── PlayerScoredFinalKill
  │             │
  │             ▼
  │        Side Effects
  │             │
  │             ├── Give/remove weapons
  │             ├── Send client messages
  │             ├── Play sounds
  │             └── Persist player statistics
  │
  └── 6. If result is ScoredFinalKill
         │
         ├── FinishGunGame()
         │      └── Restore weapons & end the mode
         │
         └── GunGameReward
                └── Grant rewards to the winner
                    and their teammates
```

## Encapsulating State-Related Side Effects

The general approach is to keep domain decisions separate from the side effects that occur as a consequence of those decisions. However, some side effects are tightly coupled to a domain object's state and its relationships, making it preferable to encapsulate them within the object.

`Flag` is an example of this. When `Flag.Capture(player)` is called, the operation changes the flag's state and carrier, while also attaching the flag to the player through SampSharp. Likewise, `Flag.Drop()` and `Flag.ReturnToBase()` remove the flag from its current carrier.

The calls to `SetAttachedObject(...)` and `RemoveAttachedObject(...)` are intentionally encapsulated within `Flag` because they provide the visual representation of the flag's relationship with its carrier. When the Carrier changes, the corresponding representation on the player must change as well. Keeping both operations together ensures that the flag's state and its representation on the carrier remain consistent.

A world pickup, however, is a different kind of representation. It represents the flag's presence in the game world rather than its relationship with the carrier. Its lifecycle is therefore handled outside `Flag`. For example, when a player takes a dropped flag, the corresponding `OnFlagTaken` event handler is responsible for removing the pickup from the world.

This introduces an intentional coupling between the `Flag` domain object and the SampSharp API. The trade-off is that the `Flag` implementation is less portable to other game frameworks, as the corresponding operations would need to be adapted to the target framework.

The coupling is therefore intentional: **keeping the flag's state and its carrier representation consistent is considered more important than keeping the domain object completely independent of the underlying game framework.**

```text
Without encapsulation

Flag.Capture(player)
      │
      ├── Flag Status = Captured
      └── Carrier = player
              │
              │  Caller must remember
              ▼
      player.SetAttachedObject(...)


With encapsulation

Flag.Capture(player)
      │
      ├── Flag Status = Captured
      ├── Carrier = player
      └── SetAttachedObject(...)
              │
              ▼
       Consistent state
```

## Map Object Definitions

Originally, map objects were defined as individual Pawn filterscripts, while the game mode itself was implemented in C# using SampSharp. When the active map changed, the game mode had to communicate with open.mp through RCON commands to load or unload the corresponding filterscript.

Map object definitions were moved to C# using [SampSharp.MapObjects](https://github.com/DevD4v3/SampSharp.MapObjects). Maps are now implemented as regular C# classes and can be loaded and unloaded directly from the game mode, without relying on Pawn filterscripts or RCON commands.

This keeps map definitions in the same language and ecosystem as the rest of the game mode, eliminating the need to maintain Pawn filterscripts and load or unload them through RCON.

## Database Independence

During development, the primary focus was implementing the game's gameplay rules. However, gameplay features still needed to work with persistent player data, including authentication and player statistics such as kills and flag captures.

The only data that needed to be persisted at that time was player-related data, while the choice of database technology was not yet relevant to the gameplay itself. The decision between SQLite and MariaDB was therefore intentionally deferred.

To keep the gameplay independent of that decision, an `IPlayerRepository` abstraction was introduced as the persistence contract. An in-memory implementation was initially used, allowing features such as retrieving and updating player statistics to be developed without coupling the game mode to a specific database technology.

Months later, SQLite and MariaDB implementations were introduced without requiring changes to the game mode core.

## Domain-Oriented Structure

The game mode originally contained a `Common` directory under `Application`, which held abstractions and utilities that were not specific to the domain being modeled, such as `Result`, `Result<T>`, and `IPasswordHasher`.

These utilities and abstractions were moved into a separate package, [GameMode.Common](https://github.com/DevD4v3/GameMode.Common), maintained independently from the game mode. The package was originally extracted from Capture the Flag to host functionality that is not specific to a single game mode and can be reused across multiple [SampSharp](https://github.com/ikkentim/SampSharp) projects.

```text
Before

Application/
├── Common/        ← Non-domain abstractions
├── GunGames/
├── Players/
├── Teams/
└── Maps/

                ↓ Architectural Decision

After

Application/
├── GunGames/     ← Domain concepts
├── Players/
├── Teams/
└── Maps/

GameMode.Common   ← Shared abstractions
```

The primary motivation was to keep the game mode structure focused on concepts from the problem domain. Instead of exposing generic folders such as `Common` within `Application`, the structure should communicate the concepts the game mode is responsible for, such as `GunGames`, `Players`, `Teams`, and `Maps`.