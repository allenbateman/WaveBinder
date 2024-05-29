using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class PowerOfTwoSliderAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(PowerOfTwoSliderAttribute))]
public class PowerOfTwoSliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            int intValue = EditorGUI.IntSlider(position, label, property.intValue, 64, 512);
            property.intValue = Mathf.ClosestPowerOfTwo(Mathf.Clamp(intValue, 64, 512));
        }
        else
        {
            EditorGUI.LabelField(position, label, "Use PowerOfTwoSlider with int.");
        }
    }
}
#endif