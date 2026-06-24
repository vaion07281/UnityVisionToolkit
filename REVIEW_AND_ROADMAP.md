# UnityVisionToolkit - Review and v1.0.0 Roadmap

This document serves as an evaluation of the current state of **UnityVisionToolkit** from the perspective of an open-source, genre-agnostic Unity package, and outlines a roadmap to reach a stable `v1.0.0` release.

---

## 1. Architectural Review

### 1.1 Runtime Architecture
- **Current State**: The toolkit relies heavily on the `Singleton<T>` pattern (`AudioManager`, `SceneLoader`, `UIManager`). While this provides quick, out-of-the-box functionality, it inherently couples systems and can hinder testability and scalability in larger projects. The `EventBus` provides a nice decoupling mechanism.
- **Evaluation**: For a lightweight toolkit, singletons are acceptable, but to scale across large, generic projects, moving toward a Service Locator or providing Dependency Injection (DI) friendly interfaces would improve the architecture.
- **Verdict**: The runtime foundation is solid for prototypes and small projects but needs a refactor toward interface-driven design for broader applicability.

### 1.2 Editor Tooling
- **Current State**: Non-existent. The package relies completely on standard Unity Inspector behaviors. Some fields use `[Tooltip("Debug info: Read-only")]` to expose state, but they can still be modified by the user.
- **Evaluation**: A high-quality public package needs to provide an excellent developer experience. Custom Editor windows, Property Drawers, and validation tools are necessary.
- **Verdict**: Needs significant additions.

### 1.3 Package Structure
- **Current State**: Currently only contains a `Runtime/` folder. It lacks standard UPM folders like `Editor/`, `Tests/`, `Samples~/`, and `Documentation~/`.
- **Evaluation**: To be a professional open-source package, it must follow the official Unity Package Manager (UPM) layout.
- **Verdict**: Requires restructuring to meet UPM standards.

### 1.4 Assembly Definitions
- **Current State**: A single `UnityVisionToolkit.Runtime.asmdef` exists.
- **Evaluation**: The setup is basic but correct for a runtime-only package. However, without an `Editor` assembly, adding editor tooling is impossible without polluting the runtime. Testing assemblies are also missing.
- **Verdict**: Needs additional `.asmdef` files for Editor, Tests, and potentially splitting out independent modules.

### 1.5 Dependency Management
- **Current State**: Zero external dependencies (`package.json` has an empty dependencies object). Uses Unity's built-in `UnityEngine.Pool`.
- **Evaluation**: This is excellent. Zero-dependency packages are highly desirable as they reduce version conflicts.
- **Verdict**: Keep this philosophy. Only add dependencies if absolutely necessary (e.g., Unity UI or Input System), and make them optional if possible.

### 1.6 Extensibility
- **Current State**: The `EventBus` is highly extensible. However, systems like `UIManager` automatically scan children via `GetComponentsInChildren`, which forces a specific scene hierarchy and limits extensibility (e.g., loading UI via Addressables).
- **Evaluation**: Systems need to be more decoupled from specific Unity workflows to support different project setups (e.g., Prefab loading vs. Scene hierarchy).
- **Verdict**: Needs refactoring to expose registration APIs (e.g., `UIManager.RegisterPanel`).

### 1.7 API Consistency
- **Current State**: Generally good. Most systems use a similar initialization flow. Events are appropriately struct-based.
- **Evaluation**: Some inconsistencies exist (e.g., `SceneLoader` handles its own UI canvas creation, while `UIManager` expects panels in the hierarchy).
- **Verdict**: APIs should be reviewed to ensure a unified configuration and initialization approach.

### 1.8 Naming Conventions
- **Current State**: Follows standard C# conventions (PascalCase for classes/methods, camelCase for parameters, `_camelCase` for private fields).
- **Evaluation**: Good. The flat namespace `UnityVisionToolkit.Runtime` aligns with the project's memory guidelines.
- **Verdict**: Maintain current conventions.

### 1.9 Generic Reusability
- **Current State**: `StateMachine`, `EventBus`, and `Singleton` are highly reusable. However, `TurnBasedStateMachine`, `TurnBasedState`, and `BattleEvents` are explicitly tied to RPG mechanics.
- **Evaluation**: A core toolkit should be genre-agnostic. RPG-specific logic alienates users building platformers, idle games, or strategy games.
- **Verdict**: Genre-specific code must be moved out of the core package.

### 1.10 Future Scalability
- **Current State**: The current structure is monolithic within the `Runtime` folder.
- **Evaluation**: As the toolkit grows, managing a single runtime assembly may increase compile times and force users to import code they don't use.
- **Verdict**: The package should be designed modularly, allowing developers to opt-in to specific systems, or separating the toolkit into multiple packages later.

---

## 2. v1.0.0 Roadmap

To prepare UnityVisionToolkit for a v1.0.0 public release, the following roadmap should be executed. The focus is on making the package a robust, genre-agnostic foundation.

### 2.1 What Should Stay (The Core)
- **`Singleton<T>`**: Keep as a basic utility, but document its use cases and limitations.
- **`EventBus`**: Keep the thread-safe, struct-based implementation. It is an excellent decoupling tool.
- **`GameObjectPool`**: Keep, as wrapping Unity's `ObjectPool` in a MonoBehaviour is a common need.
- **`StateMachine<T>` & `State<T>`**: Keep the generic FSM implementation, as it is genre-agnostic and universally useful.

### 2.2 What Should Be Refactored
- **Package Structure**: Overhaul the repository to follow standard UPM structure:
  - `Runtime/` (Core scripts)
  - `Editor/` (Custom tooling)
  - `Tests/` (Runtime and Editor unit tests)
  - `Samples~/` (Example implementations, ignored by default on import)
  - `Documentation~/` (Markdown docs, DocFX generation)
- **Assembly Definitions**:
  - Add `UnityVisionToolkit.Editor.asmdef`.
  - Add `UnityVisionToolkit.Tests.Runtime.asmdef` and `UnityVisionToolkit.Tests.Editor.asmdef`.
- **`UIManager`**: Refactor to support explicit registration (e.g., `RegisterPanel(UIPanel panel)`) instead of relying solely on `GetComponentsInChildren`. This supports dynamic UI instantiation (e.g., Addressables).
- **`SceneLoader`**: Extract the hardcoded UI creation logic into an overridable or modular system so developers can provide their own transition screens easily.
- **`AudioManager`**: Abstract to support different audio backends or multiple music tracks/layers (e.g., Music, SFX, UI, Voice) via a standard Mixer setup.

### 2.3 What Should Be Removed (From Core)
- **`TurnBasedStateMachine.cs`**
- **`TurnBasedState.cs`**
- **`BattleEvents.cs`**
- *Action*: These files are genre-specific (RPG) and violate the goal of a generic foundation. They should be moved into the `Samples~/` folder as an example of how to extend the core `StateMachine<T>`, or extracted into a completely separate optional package (e.g., `UnityVisionToolkit.RPG`).

### 2.4 What Should Be Added
- **Editor Tooling**:
  - **Read-Only Property Drawer**: Create a custom `[ReadOnly]` attribute and corresponding `PropertyDrawer` in the `Editor` assembly to replace the current `[Tooltip("Debug info: Read-only")]` workaround, ensuring variables truly cannot be modified in the Inspector.
  - **Toolkit Dashboard Window**: A centralized `EditorWindow` to manage toolkit settings, view active EventBus subscriptions, and access documentation.
  - **Menu Items**: Quick-create menu items for common patterns (e.g., `GameObject/UnityVisionToolkit/UI Panel`).
- **Interfaces for Core Systems**: Introduce interfaces like `IAudioManager`, `ISceneLoader`, and `IUIManager`. This allows the singletons to be easily mocked in tests or replaced by a Service Locator / DI framework in the future.
- **Comprehensive Unit Tests**: Implement Unity Test Framework tests for the `EventBus`, `StateMachine`, and `GameObjectPool` to guarantee stability for open-source consumers.
- **Documentation**: Add standard UPM package documentation in the `Documentation~/` folder, including a generated API reference.
