namespace UnityVisionToolkit.Runtime
{
    public class EventNodeProcessor : INodeProcessor
    {
        public void Process(BaseDialogueNode node, DialogueSession session)
        {
            if (node is EventNode eventNode)
            {
                EventBus.Raise(new DialogueEvent
                {
                    EventId = eventNode.EventId,
                    Payload = eventNode.Payload
                });

                session.TransitionToNode(eventNode.NextNodeId);
            }
        }
    }
}
