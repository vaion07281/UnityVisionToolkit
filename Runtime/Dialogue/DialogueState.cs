using System;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Represents a serializable snapshot of a dialogue session's state.
    /// Extension point for Save/Load systems.
    /// </summary>
    [Serializable]
    public class DialogueState
    {
        /// <summary>
        /// The ID of the node currently being executed.
        /// </summary>
        public string CurrentNodeId;
    }
}
