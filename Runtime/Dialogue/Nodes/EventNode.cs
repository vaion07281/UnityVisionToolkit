using System;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Triggers an event through the EventBus during dialogue flow.
    /// Does not wait for a response, just transitions to the next node.
    /// </summary>
    [Serializable]
    public class EventNode : BaseDialogueNode
    {
        public string EventId;
        public string Payload;

        /// <summary>
        /// The node to transition to immediately after firing the event.
        /// </summary>
        public string NextNodeId;
    }
}
