using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Manages the runtime state and flow of a dialogue sequence.
    /// Emits events via standard C# events for UI integrations.
    /// Does not depend on any specific UI framework.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        private DialogueGraph _currentGraph;
        private BaseDialogueNode _currentNode;
        private DialogueContext _currentContext;

        /// <summary>
        /// Triggered when the dialogue starts.
        /// </summary>
        public event Action OnDialogueStarted;

        /// <summary>
        /// Triggered when the dialogue finishes.
        /// </summary>
        public event Action OnDialogueEnded;

        /// <summary>
        /// Triggered when a new regular dialogue node is entered.
        /// </summary>
        public event Action<DialogueNode> OnDialogueNodeEnter;

        /// <summary>
        /// Triggered when the flow reaches multiple choices.
        /// Only choices whose conditions pass are provided in the list.
        /// </summary>
        public event Action<List<ChoiceNode>> OnChoicesAvailable;

        /// <summary>
        /// True if a dialogue is currently active.
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Starts the execution of a dialogue graph.
        /// </summary>
        /// <param name="graph">The graph to execute.</param>
        /// <param name="context">Optional context to evaluate conditions.</param>
        public void StartDialogue(DialogueGraph graph, DialogueContext context = null)
        {
            if (graph == null)
            {
                Debug.LogWarning("[DialogueManager] Cannot start dialogue: Graph is null.");
                return;
            }

            _currentGraph = graph;
            _currentContext = context ?? new DialogueContext();
            IsPlaying = true;

            EventBus.Raise(new DialogueStartedEvent());
            OnDialogueStarted?.Invoke();

            TransitionToNode(graph.StartNodeId);
        }

        /// <summary>
        /// Progresses the dialogue to the next node.
        /// Generally called when the player clicks to continue on a standard DialogueNode.
        /// </summary>
        public void Next()
        {
            if (!IsPlaying || _currentNode == null) return;

            if (_currentNode is DialogueNode dialogueNode)
            {
                if (dialogueNode.NextNodeIds == null || dialogueNode.NextNodeIds.Count == 0)
                {
                    EndDialogue();
                }
                else
                {
                    var firstNextNode = _currentGraph.GetNode(dialogueNode.NextNodeIds[0]);
                    if (dialogueNode.NextNodeIds.Count == 1 && !(firstNextNode is ChoiceNode))
                    {
                        TransitionToNode(dialogueNode.NextNodeIds[0]);
                    }
                    else
                    {
                        // Multiple next nodes or a single choice node
                        ProcessAvailableChoices(dialogueNode.NextNodeIds);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"[DialogueManager] Next() called, but current node is not a DialogueNode (it's {_currentNode.GetType().Name}).");
            }
        }

        /// <summary>
        /// Selects a choice and transitions to its target node.
        /// </summary>
        /// <param name="choiceNodeId">The ID of the ChoiceNode selected.</param>
        public void SelectChoice(string choiceNodeId)
        {
            if (!IsPlaying) return;

            var node = _currentGraph.GetNode(choiceNodeId);
            if (node is ChoiceNode choiceNode)
            {
                TransitionToNode(choiceNode.NextNodeId);
            }
            else
            {
                Debug.LogWarning($"[DialogueManager] Attempted to select invalid choice node: {choiceNodeId}");
            }
        }

        /// <summary>
        /// Skips the dialogue and ends it immediately.
        /// </summary>
        public void Skip()
        {
            if (!IsPlaying) return;
            EndDialogue();
        }

        /// <summary>
        /// Ends the current dialogue sequence.
        /// </summary>
        public void EndDialogue()
        {
            if (!IsPlaying) return;

            IsPlaying = false;
            _currentNode = null;
            _currentGraph = null;
            _currentContext = null;

            EventBus.Raise(new DialogueEndedEvent());
            OnDialogueEnded?.Invoke();
        }

        private void TransitionToNode(string nodeId)
        {
            var node = _currentGraph.GetNode(nodeId);
            if (node == null)
            {
                Debug.LogWarning($"[DialogueManager] Node ID '{nodeId}' not found. Ending dialogue.");
                EndDialogue();
                return;
            }

            _currentNode = node;
            ProcessCurrentNode();
        }

        private void ProcessCurrentNode()
        {
            if (_currentNode is EndNode)
            {
                EndDialogue();
            }
            else if (_currentNode is DialogueNode dialogueNode)
            {
                OnDialogueNodeEnter?.Invoke(dialogueNode);
            }
            else if (_currentNode is ConditionNode conditionNode)
            {
                bool allMet = true;
                if (conditionNode.Conditions != null)
                {
                    foreach (var condition in conditionNode.Conditions)
                    {
                        if (condition != null && !condition.Evaluate(_currentContext))
                        {
                            allMet = false;
                            break;
                        }
                    }
                }

                string nextId = allMet ? conditionNode.TrueNodeId : conditionNode.FalseNodeId;
                TransitionToNode(nextId);
            }
            else if (_currentNode is EventNode eventNode)
            {
                EventBus.Raise(new DialogueEvent
                {
                    EventId = eventNode.EventId,
                    Payload = eventNode.Payload
                });

                TransitionToNode(eventNode.NextNodeId);
            }
            else if (_currentNode is ChoiceNode)
            {
                // This shouldn't normally be entered directly via TransitionToNode
                // Choice nodes are evaluated by ProcessAvailableChoices
                Debug.LogWarning("[DialogueManager] Transitioned directly to a ChoiceNode. This is usually unintended.");
            }
        }

        private void ProcessAvailableChoices(List<string> choiceIds)
        {
            var availableChoices = new List<ChoiceNode>();

            foreach (var id in choiceIds)
            {
                var node = _currentGraph.GetNode(id);
                if (node is ChoiceNode choice)
                {
                    bool canShow = true;
                    if (choice.Conditions != null)
                    {
                        foreach (var cond in choice.Conditions)
                        {
                            if (cond != null && !cond.Evaluate(_currentContext))
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
                Debug.LogWarning("[DialogueManager] No valid choices available. Ending dialogue.");
                EndDialogue();
            }
        }
    }
}
