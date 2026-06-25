using System;

namespace UnityVisionToolkit.Runtime
{
    public class MessageNodeProcessor : INodeProcessor
    {
        public void Process(BaseDialogueNode node, DialogueSession session)
        {
            if (node is MessageNode messageNode)
            {
                session.NotifyMessageNodeEnter(messageNode);
            }
        }
    }
}
