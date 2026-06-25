using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Pure C# class managing the runtime core logic and flow of a dialogue sequence.
    /// </summary>
    public class DialogueSession
    {
        private readonly NodeProcessorRegistry _registry;
        private DialogueGraph _graph;

        public DialogueContext Context { get; private set; }
        public BaseDialogueNode CurrentNode { get; private set; }
        public bool IsPlaying { get; private set; }

        public event Action OnDialogueStarted;
        public event Action OnDialogueEnded;
        public event Action<MessageNode> OnMessageNodeEnter;
        public event Action<List<ChoiceNode>> OnChoicesAvailable;

        /// <summary>
        /// Creates a new DialogueSession using default processors.
        /// </summary>
        public DialogueSession()
        {
            _registry = NodeProcessorRegistry.CreateDefault();
        }

        /// <summary>
        /// Constructor to allow injecting a custom registry for extension and testing.
        /// </summary>
        public DialogueSession(NodeProcessorRegistry registry)
        {
            _registry = registry ?? NodeProcessorRegistry.CreateDefault();
        }

        public void StartDialogue(DialogueGraph graph, DialogueContext context = null)
        {
            if (graph == null)
            {
                Debug.LogWarning("[DialogueSession] Cannot start dialogue: Graph is null.");
                return;
            }

            _graph = graph;
            _graph.InitializeCache();
            Context = context ?? new DialogueContext();
            IsPlaying = true;

            EventBus.Raise(new DialogueStartedEvent());
            OnDialogueStarted?.Invoke();

            TransitionToNode(graph.StartNodeId);
        }

        /// <summary>
        /// Retrieves a snapshot of the current dialogue state.
        /// Useful for Save/Load systems.
        /// </summary>
        public DialogueState GetState()
        {
            if (!IsPlaying || CurrentNode == null)
                return null;

            return new DialogueState
            {
                CurrentNodeId = CurrentNode.NodeId
            };
        }

        public void TransitionToNode(string nodeId)
        {
            var node = _graph?.GetNode(nodeId);
            if (node == null)
            {
                Debug.LogWarning($"[DialogueSession] Node ID '{nodeId}' not found. Ending dialogue.");
                EndDialogue();
                return;
            }

            CurrentNode = node;
            ProcessCurrentNode();
        }

        private void ProcessCurrentNode()
        {
            if (CurrentNode == null) return;

            var processor = _registry.GetProcessor(CurrentNode.GetType());
            if (processor != null)
            {
                processor.Process(CurrentNode, this);
            }
            else
            {
                Debug.LogError($"[DialogueSession] No processor found for node type: {CurrentNode.GetType().Name}");
            }
        }

        public void NotifyMessageNodeEnter(MessageNode node)
        {
            OnMessageNodeEnter?.Invoke(node);
        }

        public void Next()
        {
            if (!IsPlaying || CurrentNode == null) return;

            if (CurrentNode is MessageNode messageNode)
            {
                if (messageNode.NextNodeIds == null || messageNode.NextNodeIds.Count == 0)
                {
                    EndDialogue();
                }
                else
                {
                    var firstNextNode = _graph.GetNode(messageNode.NextNodeIds[0]);
                    if (messageNode.NextNodeIds.Count == 1 && !(firstNextNode is ChoiceNode))
                    {
                        TransitionToNode(messageNode.NextNodeIds[0]);
                    }
                    else
                    {
                        // Multiple next nodes or a single choice node
                        ProcessAvailableChoices(messageNode.NextNodeIds);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"[DialogueSession] Next() called, but current node is not a MessageNode (it's {CurrentNode.GetType().Name}).");
            }
        }

        public void SelectChoice(string choiceNodeId)
        {
            if (!IsPlaying) return;

            var node = _graph.GetNode(choiceNodeId);
            if (node is ChoiceNode choiceNode)
            {
                TransitionToNode(choiceNode.NextNodeId);
            }
            else
            {
                Debug.LogWarning($"[DialogueSession] Attempted to select invalid choice node: {choiceNodeId}");
            }
        }

        public void Skip()
        {
            if (!IsPlaying) return;
            EndDialogue();
        }

        public void EndDialogue()
        {
            if (!IsPlaying) return;

            IsPlaying = false;
            CurrentNode = null;
            _graph = null;
            Context = null;

            EventBus.Raise(new DialogueEndedEvent());
            OnDialogueEnded?.Invoke();
        }

        private void ProcessAvailableChoices(List<string> choiceIds)
        {
            var availableChoices = new List<ChoiceNode>();

            for (int i = 0; i < choiceIds.Count; i++)
            {
                var id = choiceIds[i];
                var node = _graph.GetNode(id);
                if (node is ChoiceNode choice)
                {
                    bool canShow = true;
                    if (choice.Conditions != null)
                    {
                        for (int j = 0; j < choice.Conditions.Count; j++)
                        {
                            var cond = choice.Conditions[j];
                            if (cond != null && !cond.Evaluate(Context))
                            {
                                canShow = false;
                                break;
                            }
                        }
                    }

                    if (canShow)
                    {
                        availableChoices.Add(choice);
                    }
                }
            }

            if (availableChoices.Count > 0)
            {
                OnChoicesAvailable?.Invoke(availableChoices);
            }
            else
            {
                Debug.LogWarning("[DialogueSession] No valid choices available. Ending dialogue.");
                EndDialogue();
            }
        }
    }
}
