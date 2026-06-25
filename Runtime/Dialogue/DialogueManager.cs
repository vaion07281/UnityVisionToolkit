using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Unity MonoBehaviour entry point for the Dialogue Framework.
    /// Manages the lifecycle of a DialogueSession.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        private DialogueSession _session;

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
        public event Action<MessageNode> OnDialogueNodeEnter;

        /// <summary>
        /// Triggered when the flow reaches multiple choices.
        /// Only choices whose conditions pass are provided in the list.
        /// </summary>
        public event Action<List<ChoiceNode>> OnChoicesAvailable;

        /// <summary>
        /// True if a dialogue is currently active.
        /// </summary>
        public bool IsPlaying => _session != null && _session.IsPlaying;

        private void Awake()
        {
            InitializeSession();
        }

        private void InitializeSession()
        {
            _session = new DialogueSession();

            // Forward events
            _session.OnDialogueStarted += () => OnDialogueStarted?.Invoke();
            _session.OnDialogueEnded += () => OnDialogueEnded?.Invoke();
            _session.OnMessageNodeEnter += (node) => OnDialogueNodeEnter?.Invoke(node);
            _session.OnChoicesAvailable += (choices) => OnChoicesAvailable?.Invoke(choices);
        }

        /// <summary>
        /// Starts the execution of a dialogue graph.
        /// </summary>
        /// <param name="graph">The graph to execute.</param>
        /// <param name="context">Optional context to evaluate conditions.</param>
        public void StartDialogue(DialogueGraph graph, DialogueContext context = null)
        {
            if (_session == null)
            {
                InitializeSession();
            }

            _session.StartDialogue(graph, context);
        }

        /// <summary>
        /// Retrieves a snapshot of the current dialogue state.
        /// </summary>
        public DialogueState GetState()
        {
            return _session?.GetState();
        }

        /// <summary>
        /// Progresses the dialogue to the next node.
        /// Generally called when the player clicks to continue on a standard MessageNode.
        /// </summary>
        public void Next()
        {
            _session?.Next();
        }

        /// <summary>
        /// Selects a choice and transitions to its target node.
        /// </summary>
        /// <param name="choiceNodeId">The ID of the ChoiceNode selected.</param>
        public void SelectChoice(string choiceNodeId)
        {
            _session?.SelectChoice(choiceNodeId);
        }

        /// <summary>
        /// Skips the dialogue and ends it immediately.
        /// </summary>
        public void Skip()
        {
            _session?.Skip();
        }

        /// <summary>
        /// Ends the current dialogue sequence.
        /// </summary>
        public void EndDialogue()
        {
            _session?.EndDialogue();
        }
    }
}
