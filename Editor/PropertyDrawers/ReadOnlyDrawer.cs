using UnityEditor;
using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Editor
{
    /// <summary>
    /// Custom property drawer for the ReadOnly attribute.
    /// Draws the property in the inspector but prevents modification.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}