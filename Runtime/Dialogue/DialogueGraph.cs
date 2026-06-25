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

        private Dictionary<string, BaseDialogueNode> _nodeCache;

        /// <summary>
        /// Initializes the node cache for fast lookups.
        /// Should be called before traversing the graph at runtime.
        /// </summary>
        public void InitializeCache()
        {
            if (_nodeCache != null)
                return;

            _nodeCache = new Dictionary<string, BaseDialogueNode>();

            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                if (node == null || string.IsNullOrEmpty(node.NodeId))
                    continue;

                if (!_nodeCache.ContainsKey(node.NodeId))
                {
                    _nodeCache[node.NodeId] = node;
                }
                else
                {
                    Debug.LogWarning($"[DialogueGraph] Duplicate NodeId found: '{node.NodeId}'. Only the first one will be accessible.");
                }
            }
        }

        /// <summary>
        /// Retrieves a node by its ID using O(1) lookup.
        /// </summary>
        public BaseDialogueNode GetNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
                return null;

            if (_nodeCache == null)
            {
                Debug.LogWarning("[DialogueGraph] Cache not initialized. Initializing now, but it's recommended to call InitializeCache() before starting the dialogue.");
                InitializeCache();
            }

            if (_nodeCache.TryGetValue(nodeId, out var node))
            {
                return node;
            }

            return null;
        }

        private void OnDisable()
        {
            _nodeCache = null;
        }
    }
}
