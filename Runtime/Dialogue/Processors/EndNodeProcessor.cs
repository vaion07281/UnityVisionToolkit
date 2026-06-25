namespace UnityVisionToolkit.Runtime
{
    public class EndNodeProcessor : INodeProcessor
    {
        public void Process(BaseDialogueNode node, DialogueSession session)
        {
            session.EndDialogue();
        }
    }
}
