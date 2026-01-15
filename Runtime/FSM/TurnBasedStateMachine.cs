using System.Collections;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// A Finite State Machine optimized for Turn-Based RPGs.
    /// Manages Coroutine-based states and integrates with the EventBus.
    /// </summary>
    /// <typeparam name="T">The Controller type (must be a MonoBehaviour).</typeparam>
    public class TurnBasedStateMachine<T> where T : MonoBehaviour
    {
        private T _owner;
        private Coroutine _currentRoutine;

        /// <summary>
        /// Gets the current active state.
        /// </summary>
        public TurnBasedState<T> CurrentState { get; private set; }

        /// <summary>
        /// Indicates if a battle is currently in progress.
        /// </summary>
        public bool IsBattleActive { get; private set; }

        public TurnBasedStateMachine(T owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Starts the battle sequence with the given state.
        /// </summary>
        /// <param name="startingState">The initial state.</param>
        public void StartBattle(TurnBasedState<T> startingState)
        {
            if (IsBattleActive)
            {
                Debug.LogWarning("[TurnBasedStateMachine] Battle is already active. Restarting...");
                EndBattle(false); // Reset if needed
            }

            IsBattleActive = true;
            SwitchState(startingState);
        }

        /// <summary>
        /// Transitions to the next turn state.
        /// </summary>
        /// <param name="nextState">The next state to run.</param>
        public void NextTurn(TurnBasedState<T> nextState)
        {
            if (!IsBattleActive)
            {
                Debug.LogWarning("[TurnBasedStateMachine] Cannot switch turn; battle is not active.");
                return;
            }
            SwitchState(nextState);
        }

        /// <summary>
        /// Ends the battle, cleaning up the current state and stopping coroutines.
        /// </summary>
        /// <param name="isWin">True if the player won, false otherwise.</param>
        public void EndBattle(bool isWin)
        {
            if (!IsBattleActive) return;

            // Cleanup current state
            if (CurrentState != null)
            {
                CurrentState.Exit();
                CurrentState = null;
            }

            // Stop any running routine
            if (_currentRoutine != null)
            {
                _owner.StopCoroutine(_currentRoutine);
                _currentRoutine = null;
            }

            IsBattleActive = false;
            EventBus.Raise(new BattleEndedEvent(isWin));
        }

        private void SwitchState(TurnBasedState<T> newState)
        {
            // 1. Exit old state
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            // 2. Stop old routine
            if (_currentRoutine != null)
            {
                _owner.StopCoroutine(_currentRoutine);
                _currentRoutine = null;
            }

            // 3. Swap state
            CurrentState = newState;

            if (CurrentState != null)
            {
                // 4. Raise Event
                // Use the type name (e.g., "PlayerTurnState") as the identifier
                EventBus.Raise(new BattleStateChangedEvent(CurrentState.GetType().Name));

                // 5. Enter new state
                CurrentState.Enter();

                // 6. Start new routine
                _currentRoutine = _owner.StartCoroutine(CurrentState.Execute());
            }
        }
    }
}
