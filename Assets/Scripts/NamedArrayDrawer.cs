


using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NamedArrayAttribute))]
public class NamedArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        try
        {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            bool value = EditorGUI.Toggle(rect, new GUIContent(((NamedArrayAttribute)attribute).names[pos]), property.boolValue); // Use EditorGUI.Toggle to display a boolean field
            property.boolValue = value;
        }
        catch
        {
            EditorGUI.PropertyField(rect, property, label);
        }
    }
}
