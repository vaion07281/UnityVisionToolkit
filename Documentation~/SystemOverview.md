# System Overview

The Unity Vision Toolkit provides a suite of genre-agnostic tools and patterns to accelerate your Unity development.

## Core Patterns

* **Singleton**: Thread-safe implementations for global access points (`Singleton<T>`).
* **EventBus**: A type-safe, static event broker for decoupled communication across systems. Includes interactive playable sample scenes generated at runtime.
* **GameObjectPool**: A wrapper around `UnityEngine.Pool` to efficiently manage and reuse GameObjects, avoiding garbage collection spikes. Includes interactive playable sample scenes generated at runtime.
* **StateMachine (FSM)**: A lightweight state machine for handling complex logic states easily. Includes interactive playable sample scenes generated at runtime.

## Core Systems

* **UIManager**: A stack-based UI manager to handle pushing, popping, and navigating through different UI panels. Includes interactive playable sample scenes generated at runtime.
* **AudioManager**: A centralized system to handle playing music and sound effects, utilizing object pooling for SFX.
* **SceneLoader**: An asynchronous scene loading system with built-in transition overlays and event broadcasts.

## Utilities

* **ReadOnly Attribute**: A `[ReadOnly]` attribute used to expose debug information in the Unity Inspector without allowing it to be modified.

*(Note: RPG-specific systems are also included for backward compatibility but are not the primary focus of the core toolkit).*
