using System.Collections;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Abstract base state for a Turn-Based RPG system.
    /// Supports Unity Coroutines for async logic (animations, waiting).
    /// </summary>
    /// <typeparam name="T">The Controller type (must be a MonoBehaviour).</typeparam>
    public abstract class TurnBasedState<T> where T : MonoBehaviour
    {
        protected T Owner;
        protected TurnBasedStateMachine<T> StateMachine;

        /// <summary>
        /// Initializes a new instance of the TurnBasedState class.
        /// </summary>
        /// <param name="owner">The MonoBehaviour owner (e.g., BattleManager).</param>
        /// <param name="stateMachine">The state machine managing this state.</param>
        protected TurnBasedState(T owner, TurnBasedStateMachine<T> stateMachine)
        {
            Owner = owner;
            StateMachine = stateMachine;
        }

        /// <summary>
        /// Called synchronously when entering the state.
        /// Use for immediate setup (e.g., UI updates, variable resets).
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// The main execution routine of the state.
        /// Called as a Coroutine immediately after Enter().
        /// Use for async logic (e.g., waiting for animations, player input).
        /// </summary>
        public virtual IEnumerator Execute()
        {
            yield break;
        }

        /// <summary>
        /// Called synchronously when exiting the state.
        /// Use for cleanup (e.g., disabling UI).
        /// </summary>
        public virtual void Exit() { }
    }
}
