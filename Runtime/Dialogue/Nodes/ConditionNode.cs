using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Evaluates conditions to automatically branch the dialogue flow.
    /// </summary>
    [Serializable]
    public class ConditionNode : BaseDialogueNode
    {
        /// <summary>
        /// Conditions to evaluate. Usually all must be true.
        /// </summary>
        [SerializeReference]
        public List<IDialogueCondition> Conditions = new List<IDialogueCondition>();

        /// <summary>
        /// The node to transition to if the condition is true.
        /// </summary>
        public string TrueNodeId;

        /// <summary>
        /// The node to transition to if the condition is false.
        /// </summary>
        public string FalseNodeId;
    }
}
