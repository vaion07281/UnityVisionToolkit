namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Event dispatched by the EventNode through the EventBus.
    /// Games can listen to this event to trigger actions like starting a battle or giving an item.
    /// </summary>
    public struct DialogueEvent
    {
        /// <summary>
        /// The unique identifier or name of the event.
        /// </summary>
        public string EventId;

        /// <summary>
        /// Optional string payload for additional data (e.g., JSON, or simple values).
        /// </summary>
        public string Payload;
    }
}
