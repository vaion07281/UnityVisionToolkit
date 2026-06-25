# System Overview

The Unity Vision Toolkit provides a suite of genre-agnostic tools and patterns to accelerate your Unity development.

## Core Patterns

* **Singleton**: Thread-safe implementations for global access points (`Singleton<T>`).
* **EventBus**: A type-safe, static event broker for decoupled communication across systems.
* **GameObjectPool**: A wrapper around `UnityEngine.Pool` to efficiently manage and reuse GameObjects, avoiding garbage collection spikes.
* **StateMachine (FSM)**: A lightweight state machine for handling complex logic states easily.

## Core Systems

* **UIManager**: A stack-based UI manager to handle pushing, popping, and navigating through different UI panels.
* **AudioManager**: A centralized system to handle playing music and sound effects, utilizing object pooling for SFX.
* **SceneLoader**: An asynchronous scene loading system with built-in transition overlays and event broadcasts.

## Utilities

* **ReadOnly Attribute**: A `[ReadOnly]` attribute used to expose debug information in the Unity Inspector without allowing it to be modified.

*(Note: RPG-specific systems are also included for backward compatibility but are not the primary focus of the core toolkit).*

## Examples

To see these systems in action, check out the `Samples~` folder. Each sample includes an interactive `.unity` scene demonstrating its behavior:
* **EventBusSampleScene**: Demonstrates publishing and subscribing to events using UI buttons.
* **StateMachineSampleScene**: Demonstrates transitioning between different states dynamically.
* **ObjectPoolSampleScene**: Demonstrates fetching and releasing GameObjects efficiently without instantiating them repeatedly.
* **UIManagerSampleScene**: Demonstrates pushing and popping UI panels from the navigation stack.