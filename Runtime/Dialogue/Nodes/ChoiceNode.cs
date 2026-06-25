using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Represents a single choice presented to the player.
    /// Can have conditions that dictate whether the choice is valid or visible.
    /// </summary>
    [Serializable]
    public class ChoiceNode : BaseDialogueNode
    {
        public string ChoiceText;

        /// <summary>
        /// The node to transition to if this choice is selected.
        /// </summary>
        public string NextNodeId;

        /// <summary>
        /// Conditions to evaluate to determine if this choice is available.
        /// </summary>
        [SerializeReference]
        public List<IDialogueCondition> Conditions = new List<IDialogueCondition>();
    }
}
