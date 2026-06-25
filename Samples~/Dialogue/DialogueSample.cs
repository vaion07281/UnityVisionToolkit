using System.Collections.Generic;
using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    /// <summary>
    /// Demonstrates how to use the Dialogue Framework via code.
    /// This sample uses Debug.Log to simulate a UI.
    /// Press Space to advance dialogue, or Alpha1/Alpha2 to select choices.
    /// </summary>
    public class DialogueSample : MonoBehaviour
    {
        private DialogueManager _manager;
        private DialogueGraph _graph;
        private List<ChoiceNode> _currentChoices;
        private bool _awaitingInput;

        public void Initialize(DialogueManager manager, DialogueGraph graph)
        {
            _manager = manager;
            _graph = graph;

            // Subscribe to DialogueManager events
            _manager.OnDialogueStarted += OnDialogueStarted;
            _manager.OnDialogueEnded += OnDialogueEnded;
            _manager.OnDialogueNodeEnter += OnNodeEntered;
            _manager.OnChoicesAvailable += OnChoicesAvailable;

            // Subscribe to framework events (e.g. from EventNode)
            EventBus.Subscribe<DialogueEvent>(OnDialogueEventFired);

            Debug.Log("<color=cyan>Dialogue Sample Initialized. Press [SPACE] to start dialogue.</color>");
        }

        private void OnDestroy()
        {
            if (_manager != null)
            {
                _manager.OnDialogueStarted -= OnDialogueStarted;
                _manager.OnDialogueEnded -= OnDialogueEnded;
                _manager.OnDialogueNodeEnter -= OnNodeEntered;
                _manager.OnChoicesAvailable -= OnChoicesAvailable;
            }

            EventBus.Unsubscribe<DialogueEvent>(OnDialogueEventFired);
        }

        private void Update()
        {
            if (_manager == null) return;

            if (!_manager.IsPlaying)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _manager.StartDialogue(_graph);
                }
                return;
            }

            if (_awaitingInput)
            {
                // If we are waiting for a choice
                if (_currentChoices != null && _currentChoices.Count > 0)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1) && _currentChoices.Count >= 1)
                    {
                        SelectChoice(0);
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2) && _currentChoices.Count >= 2)
                    {
                        SelectChoice(1);
                    }
                }
                // If we are just waiting to proceed to next line
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        _manager.Next();
                    }
                }
            }
        }

        private void SelectChoice(int index)
        {
            var choice = _currentChoices[index];
            Debug.Log($"<color=green>[Player Selected]: {choice.ChoiceText}</color>");
            _currentChoices = null;
            _manager.SelectChoice(choice.NodeId);
        }

        private void OnDialogueStarted()
        {
            Debug.Log("<color=yellow>--- Dialogue Started ---</color>");
        }

        private void OnDialogueEnded()
        {
            _awaitingInput = false;
            _currentChoices = null;
            Debug.Log("<color=yellow>--- Dialogue Ended ---</color>");
            Debug.Log("<color=cyan>Press [SPACE] to restart dialogue.</color>");
        }

        private void OnNodeEntered(DialogueNode node)
        {
            _currentChoices = null;
            _awaitingInput = true;
            Debug.Log($"<b>[{node.Speaker}]</b>: {node.Content}");
            Debug.Log("<i>(Press SPACE to continue...)</i>");
        }

        private void OnChoicesAvailable(List<ChoiceNode> choices)
        {
            _currentChoices = choices;
            _awaitingInput = true;
            Debug.Log("<b>Choices:</b>");
            for (int i = 0; i < choices.Count; i++)
            {
                Debug.Log($"  [{i + 1}] {choices[i].ChoiceText}");
            }
            Debug.Log("<i>(Press 1 or 2 to select...)</i>");
        }

        private void OnDialogueEventFired(DialogueEvent evt)
        {
            Debug.Log($"<color=orange>[Event Fired via EventBus]: ID = {evt.EventId}, Payload = {evt.Payload}</color>");
        }
    }
}
