namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Abstract base class for all states in the Finite State Machine.
    /// </summary>
    /// <typeparam name="T">The type of the Controller/Owner of the state machine.</typeparam>
    public abstract class State<T>
    {
        protected T Owner;
        protected StateMachine<T> StateMachine;

        /// <summary>
        /// Initializes a new instance of the State class.
        /// </summary>
        /// <param name="owner">The owner of this state.</param>
        /// <param name="stateMachine">The state machine managing this state.</param>
        protected State(T owner, StateMachine<T> stateMachine)
        {
            Owner = owner;
            StateMachine = stateMachine;
        }

        /// <summary>
        /// Called when the state machine enters this state.
        /// Use this for setup logic (e.g., enabling UI, playing animations).
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// Called every frame while this state is active.
        /// Use this for frame-based logic (e.g., waiting for input, timers).
        /// </summary>
        public virtual void LogicUpdate() { }

        /// <summary>
        /// Called when the state machine exits this state.
        /// Use this for cleanup logic (e.g., hiding UI, resetting flags).
        /// </summary>
        public virtual void Exit() { }
    }
}
