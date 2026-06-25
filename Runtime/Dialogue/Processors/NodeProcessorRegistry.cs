using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Registry for associating Node types with their specific INodeProcessor.
    /// Provides an extensible way to add new node types without modifying core flow logic.
    /// Made public so users can implement their own nodes and processors without touching the framework source.
    /// </summary>
    public class NodeProcessorRegistry
    {
        private readonly Dictionary<Type, INodeProcessor> _processors = new Dictionary<Type, INodeProcessor>();

        public void RegisterProcessor(Type nodeType, INodeProcessor processor)
        {
            _processors[nodeType] = processor;
        }

        public INodeProcessor GetProcessor(Type nodeType)
        {
            if (_processors.TryGetValue(nodeType, out var processor))
            {
                return processor;
            }
            return null;
        }

        /// <summary>
        /// Registers default processors for the core node types.
        /// </summary>
        public static NodeProcessorRegistry CreateDefault()
        {
            var registry = new NodeProcessorRegistry();
            registry.RegisterProcessor(typeof(MessageNode), new MessageNodeProcessor());
            registry.RegisterProcessor(typeof(ConditionNode), new ConditionNodeProcessor());
            registry.RegisterProcessor(typeof(EventNode), new EventNodeProcessor());
            registry.RegisterProcessor(typeof(EndNode), new EndNodeProcessor());
            // ChoiceNode is typically not transitioned to directly but evaluated when needed.
            // However, we can add a simple warning processor or just ignore it.
            registry.RegisterProcessor(typeof(ChoiceNode), new ChoiceNodeProcessor());
            return registry;
        }
    }
}
