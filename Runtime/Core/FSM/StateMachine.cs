using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// A generic Finite State Machine that manages states of type State&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">The type of the Controller/Owner of the state machine.</typeparam>
    public class StateMachine<T>
    {
        /// <summary>
        /// Gets the current active state.
        /// </summary>
        public State<T> CurrentState { get; private set; }

        /// <summary>
        /// Initializes the State Machine with a starting state.
        /// </summary>
        /// <param name="startingState">The state to start in.</param>
        public void Initialize(State<T> startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        /// <summary>
        /// Transitions the State Machine to a new state.
        /// Calls Exit() on the current state and Enter() on the new state.
        /// </summary>
        /// <param name="newState">The state to transition to.</param>
        public void ChangeState(State<T> newState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = newState;
            CurrentState.Enter();
        }

        /// <summary>
        /// Updates the current state. Should be called every frame (e.g., from MonoBehaviour.Update).
        /// </summary>
        public void OnUpdate()
        {
            if (CurrentState != null)
            {
                CurrentState.LogicUpdate();
            }
        }
    }
}
