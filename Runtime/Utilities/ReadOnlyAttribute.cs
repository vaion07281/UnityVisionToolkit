using UnityEngine;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Attribute used to make a field read-only in the Unity Inspector.
    /// Used primarily to display debug information without allowing it to be modified.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}