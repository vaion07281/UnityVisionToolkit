using System.Collections.Generic;
using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Samples
{
    /// <summary>
    /// Bootstraps the Dialogue sample scene.
    /// Creates a DialogueManager and an in-memory DialogueGraph for demonstration purposes.
    /// </summary>
    public class DialogueSampleBootstrap : MonoBehaviour
    {
        [SerializeField] private DialogueSample _sampleScript;

        private void Start()
        {
            if (_sampleScript == null)
            {
                _sampleScript = gameObject.AddComponent<DialogueSample>();
            }

            // Create DialogueManager
            var dialogueGo = new GameObject("DialogueManager");
            var manager = dialogueGo.AddComponent<DialogueManager>();

            // Assign manager to the sample script
            _sampleScript.Initialize(manager, CreateSampleGraph());
        }

        private DialogueGraph CreateSampleGraph()
        {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.StartNodeId = "node_1";

            // Node 1: Greeting
            var node1 = new MessageNode
            {
                NodeId = "node_1",
                Speaker = "Villager",
                Content = "Hello traveler! Would you like a sword or a potion?",
                NextNodeIds = new List<string> { "choice_sword", "choice_potion" }
            };

            // Choice Nodes
            var choiceSword = new ChoiceNode { NodeId = "choice_sword", ChoiceText = "I'll take the sword.", NextNodeId = "event_sword" };
            var choicePotion = new ChoiceNode { NodeId = "choice_potion", ChoiceText = "A potion, please.", NextNodeId = "event_potion" };

            // Event Nodes (Demonstrating triggering events via EventBus)
            var eventSword = new EventNode { NodeId = "event_sword", EventId = "give_item", Payload = "sword", NextNodeId = "node_sword_reply" };
            var eventPotion = new EventNode { NodeId = "event_potion", EventId = "give_item", Payload = "potion", NextNodeId = "node_potion_reply" };

            // Dialogue Nodes (Reply based on choice)
            var replySword = new MessageNode { NodeId = "node_sword_reply", Speaker = "Villager", Content = "A wise choice. It is dangerous to go alone!", NextNodeIds = new List<string> { "node_end" } };
            var replyPotion = new MessageNode { NodeId = "node_potion_reply", Speaker = "Villager", Content = "Stay safe! Drink this if you get hurt.", NextNodeIds = new List<string> { "node_end" } };

            // End Node
            var endNode = new EndNode { NodeId = "node_end" };

            // Add all nodes to graph
            graph.Nodes.Add(node1);
            graph.Nodes.Add(choiceSword);
            graph.Nodes.Add(choicePotion);
            graph.Nodes.Add(eventSword);
            graph.Nodes.Add(eventPotion);
            graph.Nodes.Add(replySword);
            graph.Nodes.Add(replyPotion);
            graph.Nodes.Add(endNode);

            return graph;
        }
    }
}
