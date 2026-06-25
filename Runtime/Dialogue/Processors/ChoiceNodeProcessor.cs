using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    public class ChoiceNodeProcessor : INodeProcessor
    {
        public void Process(BaseDialogueNode node, DialogueSession session)
        {
            Debug.LogWarning("[DialogueSession] Transitioned directly to a ChoiceNode. This is usually unintended.");
        }
    }
}
