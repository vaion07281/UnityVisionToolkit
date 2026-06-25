namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Interface for evaluating conditions within the dialogue framework.
    /// Games should implement this to provide their own logic (e.g. check quests, items).
    /// </summary>
    public interface IDialogueCondition
    {
        /// <summary>
        /// Evaluates the condition against the provided dialogue context.
        /// </summary>
        /// <param name="context">The context for the current dialogue evaluation.</param>
        /// <returns>True if the condition is met, false otherwise.</returns>
        bool Evaluate(DialogueContext context);
    }
}
