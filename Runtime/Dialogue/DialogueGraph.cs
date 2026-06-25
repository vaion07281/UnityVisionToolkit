using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Represents a complete dialogue sequence (e.g., NPC conversation, cutscene).
    /// Stores the flow of nodes and their connections.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogueGraph", menuName = "UnityVisionToolkit/Dialogue/DialogueGraph")]
    public class DialogueGraph : ScriptableObject
    {
        public string StartNodeId;

        [SerializeReference]
        public List<BaseDialogueNode> Nodes = new List<BaseDialogueNode>();

        /// <summary>
        /// Retrieves a node by its ID.
        /// </summary>
        public BaseDialogueNode GetNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
                return null;

            return Nodes.Find(n => n.NodeId == nodeId);
        }
    }
}
