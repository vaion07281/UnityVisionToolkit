using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Represents a key-value pair for metadata associated with a node.
    /// </summary>
    [Serializable]
    public class NodeMetadata
    {
        public string Key;
        public string Value;
    }

    /// <summary>
    /// Represents a basic dialogue line with a speaker and text content.
    /// Can point to next nodes directly (e.g. following line or a list of choices).
    /// Uses string keys for resources to stay decoupled from Unity's rendering/asset APIs.
    /// </summary>
    [Serializable]
    public class MessageNode : BaseDialogueNode
    {
        public string Speaker;

        [TextArea(3, 10)]
        public string Content;

        // Decoupled resource references
        public string PortraitKey;
        public string BackgroundKey;
        public string VoiceKey;

        // Extension point for arbitrary node data
        public List<NodeMetadata> Metadata = new List<NodeMetadata>();

        /// <summary>
        /// IDs of the next nodes.
        /// Usually 1 if just continuing the flow, or multiple if pointing to ChoiceNodes.
        /// </summary>
        public List<string> NextNodeIds = new List<string>();
    }
}
