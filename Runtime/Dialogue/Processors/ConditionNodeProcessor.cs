namespace UnityVisionToolkit.Runtime
{
    public class ConditionNodeProcessor : INodeProcessor
    {
        public void Process(BaseDialogueNode node, DialogueSession session)
        {
            if (node is ConditionNode conditionNode)
            {
                bool allMet = true;
                if (conditionNode.Conditions != null)
                {
                    for (int i = 0; i < conditionNode.Conditions.Count; i++)
                    {
                        var condition = conditionNode.Conditions[i];
                        if (condition != null && !condition.Evaluate(session.Context))
                        {
                            allMet = false;
                            break;
                        }
                    }
                }

                string nextId = allMet ? conditionNode.TrueNodeId : conditionNode.FalseNodeId;
                session.TransitionToNode(nextId);
            }
        }
    }
}
