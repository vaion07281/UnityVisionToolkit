using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Tests.Runtime
{
    public class DialogueFrameworkTests
    {
        private GameObject _go;
        private DialogueManager _manager;

        // Dummy condition for testing
        private class TestCondition : IDialogueCondition
        {
            public bool Result { get; set; } = true;
            public bool Evaluate(DialogueContext context) => Result;
        }

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("DialogueManagerObj");
            _manager = _go.AddComponent<DialogueManager>();
            EventBus.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null)
                Object.DestroyImmediate(_go);
            EventBus.Clear();
        }

        [Test]
        public void DialogueManager_StartDialogue_TriggersEvents_And_EntersFirstNode()
        {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.StartNodeId = "node_1";
            var node1 = new DialogueNode { NodeId = "node_1", Content = "Hello" };
            graph.Nodes.Add(node1);

            bool startedEventFired = false;
            bool nodeEnteredFired = false;

            _manager.OnDialogueStarted += () => startedEventFired = true;
            _manager.OnDialogueNodeEnter += (n) =>
            {
                if (n.NodeId == "node_1") nodeEnteredFired = true;
            };

            _manager.StartDialogue(graph);

            Assert.IsTrue(_manager.IsPlaying);
            Assert.IsTrue(startedEventFired);
            Assert.IsTrue(nodeEnteredFired);
        }

        [Test]
        public void DialogueManager_Next_TransitionsToNextNode()
        {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.StartNodeId = "node_1";

            var node1 = new DialogueNode { NodeId = "node_1", NextNodeIds = new List<string> { "node_2" } };
            var node2 = new DialogueNode { NodeId = "node_2" };

            graph.Nodes.Add(node1);
            graph.Nodes.Add(node2);

            bool enteredNode2 = false;
            _manager.OnDialogueNodeEnter += (n) => { if (n.NodeId == "node_2") enteredNode2 = true; };

            _manager.StartDialogue(graph);
            _manager.Next();

            Assert.IsTrue(enteredNode2);
        }

        [Test]
        public void DialogueManager_ConditionNode_RoutesCorrectly()
        {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.StartNodeId = "cond_1";

            var condition = new TestCondition { Result = false };
            var condNode = new ConditionNode
            {
                NodeId = "cond_1",
                Conditions = new List<IDialogueCondition> { condition },
                TrueNodeId = "node_true",
                FalseNodeId = "node_false"
            };

            var nodeTrue = new DialogueNode { NodeId = "node_true" };
            var nodeFalse = new DialogueNode { NodeId = "node_false" };

            graph.Nodes.Add(condNode);
            graph.Nodes.Add(nodeTrue);
            graph.Nodes.Add(nodeFalse);

            string enteredNode = "";
            _manager.OnDialogueNodeEnter += (n) => enteredNode = n.NodeId;

            // Result is false, should route to node_false
            _manager.StartDialogue(graph);
            Assert.AreEqual("node_false", enteredNode);

            // Change to true and test again
            _manager.EndDialogue();
            condition.Result = true;
            _manager.StartDialogue(graph);
            Assert.AreEqual("node_true", enteredNode);
        }

        [Test]
        public void DialogueManager_EventNode_FiresEvent_And_Continues()
        {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.StartNodeId = "event_1";

            var eventNode = new EventNode
            {
                NodeId = "event_1",
                EventId = "give_item",
                Payload = "sword",
                NextNodeId = "node_1"
            };
            var node1 = new DialogueNode { NodeId = "node_1" };

            graph.Nodes.Add(eventNode);
            graph.Nodes.Add(node1);

            bool eventFired = false;
            EventBus.Subscribe<DialogueEvent>(e =>
            {
                if (e.EventId == "give_item" && e.Payload == "sword")
                    eventFired = true;
            });

            bool enteredNode1 = false;
            _manager.OnDialogueNodeEnter += (n) => { if (n.NodeId == "node_1") enteredNode1 = true; };

            _manager.StartDialogue(graph);

            Assert.IsTrue(eventFired);
            Assert.IsTrue(enteredNode1);
        }

        [Test]
        public void DialogueManager_Choices_FiltersAndPresentsChoices()
        {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.StartNodeId = "node_start";

            var startNode = new DialogueNode
            {
                NodeId = "node_start",
                NextNodeIds = new List<string> { "choice_1", "choice_2" }
            };

            var choice1 = new ChoiceNode { NodeId = "choice_1", NextNodeId = "node_end" };
            var choice2 = new ChoiceNode
            {
                NodeId = "choice_2",
                NextNodeId = "node_end",
                Conditions = new List<IDialogueCondition> { new TestCondition { Result = false } } // Will fail
            };

            var endNode = new EndNode { NodeId = "node_end" };

            graph.Nodes.Add(startNode);
            graph.Nodes.Add(choice1);
            graph.Nodes.Add(choice2);
            graph.Nodes.Add(endNode);

            List<ChoiceNode> presentedChoices = null;
            _manager.OnChoicesAvailable += (choices) => presentedChoices = choices;

            _manager.StartDialogue(graph);
            _manager.Next(); // Move from start to choices

            Assert.IsNotNull(presentedChoices);
            Assert.AreEqual(1, presentedChoices.Count);
            Assert.AreEqual("choice_1", presentedChoices[0].NodeId);

            _manager.SelectChoice("choice_1");
            Assert.IsFalse(_manager.IsPlaying); // Reached EndNode
        }

        [Test]
        public void DialogueManager_EndNode_FinishesDialogue()
        {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.StartNodeId = "end_1";

            var endNode = new EndNode { NodeId = "end_1" };
            graph.Nodes.Add(endNode);

            bool endedEventFired = false;
            _manager.OnDialogueEnded += () => endedEventFired = true;

            _manager.StartDialogue(graph);

            Assert.IsFalse(_manager.IsPlaying);
            Assert.IsTrue(endedEventFired);
        }

        [Test]
        public void DialogueManager_SingleChoiceNode_RoutesCorrectly()
        {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.StartNodeId = "node_1";

            var node1 = new DialogueNode { NodeId = "node_1", NextNodeIds = new List<string> { "choice_1" } };
            var choice1 = new ChoiceNode { NodeId = "choice_1", NextNodeId = "node_2" };
            var node2 = new DialogueNode { NodeId = "node_2" };

            graph.Nodes.Add(node1);
            graph.Nodes.Add(choice1);
            graph.Nodes.Add(node2);

            List<ChoiceNode> presentedChoices = null;
            _manager.OnChoicesAvailable += (choices) => presentedChoices = choices;

            _manager.StartDialogue(graph);
            _manager.Next();

            Assert.IsNotNull(presentedChoices);
            Assert.AreEqual(1, presentedChoices.Count);
            Assert.AreEqual("choice_1", presentedChoices[0].NodeId);
        }
    }
}
