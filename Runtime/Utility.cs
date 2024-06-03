using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WaveBinder.Runtime
{
    #region Custom Attribute
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
    #endregion

    #region Linear Interpolation
    public static class LinearInterpolation
    {
        public static (int, int) MapRange(int valueMin, int valueMax, int targetMin, int targetMax, int newMin, int newMax)
        {
            // Ensure valueMin does not exceed valueMax
            if (valueMin > valueMax)
            {
                throw new ArgumentException("valueMin cannot be greater than valueMax.");
            }

            // Ensure newMin does not exceed newMax
            if (newMin > newMax)
            {
                throw new ArgumentException("newMin cannot be greater than newMax.");
            }

            // Map valueMin to newMin and valueMax to newMax
            int mappedMin = MapValue(valueMin, targetMin, targetMax, newMin, newMax);
            int mappedMax = MapValue(valueMax, targetMin, targetMax, newMin, newMax);

            return (mappedMin, mappedMax);
        }
        private static int MapValue(int value, int targetMin, int targetMax, int newMin, int newMax)
        {
            // Linear interpolation to map the value from target range to new range
            return (int)((double)(value - targetMin) / (targetMax - targetMin) * (newMax - newMin) + newMin);
        }
    }
 
    #endregion
}