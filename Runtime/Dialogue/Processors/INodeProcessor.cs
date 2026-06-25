namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Defines the interface for processing a specific type of Dialogue Node.
    /// </summary>
    public interface INodeProcessor
    {
        /// <summary>
        /// Processes the given node.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <param name="session">The current dialogue session.</param>
        void Process(BaseDialogueNode node, DialogueSession session);
    }
}
