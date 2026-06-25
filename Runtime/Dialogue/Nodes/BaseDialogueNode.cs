using System;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Base class for all dialogue nodes.
    /// Uses [Serializable] so it can be handled by [SerializeReference].
    /// </summary>
    [Serializable]
    public abstract class BaseDialogueNode
    {
        /// <summary>
        /// Unique identifier for this node within the graph.
        /// </summary>
        public string NodeId;
    }
}
