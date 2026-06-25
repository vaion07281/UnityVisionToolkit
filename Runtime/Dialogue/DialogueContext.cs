using System.Collections.Generic;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// An extensible context object passed to dialogue conditions during evaluation.
    /// Games can inherit from this or populate it to provide data like Quest, Inventory, Flags, etc.
    /// </summary>
    public class DialogueContext
    {
        // Users can subclass this to add strong-typed dependencies
        // or use this generic dictionary for simple variables.
        public Dictionary<string, object> Variables { get; } = new Dictionary<string, object>();
    }
}
