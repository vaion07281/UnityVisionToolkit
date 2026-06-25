using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Represents a basic dialogue line with a speaker, content, and visual assets.
    /// Can point to next nodes directly (e.g. following line or a list of choices).
    /// </summary>
    [Serializable]
    public class DialogueNode : BaseDialogueNode
    {
        public string Speaker;

        [TextArea(3, 10)]
        public string Content;

        // Note: Sprite dependencies are fine here.
        // We do not depend on UI, just data types.
        public Sprite Portrait;
        public Sprite Background;

        /// <summary>
        /// IDs of the next nodes.
        /// Usually 1 if just continuing the flow, or multiple if pointing to ChoiceNodes.
        /// </summary>
        public List<string> NextNodeIds = new List<string>();
    }
}
