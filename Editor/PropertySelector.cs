using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;


namespace WaveBinder.Editor
{
    public class PropertySelector
    {
        static Dictionary<string, PropertySelector> _instances = new Dictionary<string, PropertySelector>();    

        public static PropertySelector GetInstance(SerializedProperty spTarget, SerializedProperty spPropertyType)
        {
            var key = spTarget.objectReferenceValue.GetType() + spPropertyType.stringValue;
             
            // try get instance from dictionary
            PropertySelector selector;
            if(_instances.TryGetValue(key, out selector))
            {
                return selector; 
            }

            // new instance
            selector = new PropertySelector(spTarget,spPropertyType);
            _instances[key] = selector;
            return selector;
        }

        #region Constructor
        PropertySelector (SerializedProperty spTarget, SerializedProperty spPropertyType)
        {
            // determine the target propety type using reflection
            _propertyType = Type.GetType(spPropertyType.stringValue);

            //Porperty names candidates
            _candidates = spTarget.objectReferenceValue.GetType().
                GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).
                Where(prop => prop.PropertyType == _propertyType).
                Select(prop => prop.Name).ToArray();
        }
        Type _propertyType;
        string[] _candidates;
        #endregion

        #region GUI implementation
        public bool ShowGUI(SerializedProperty spPropertyName)
        {
            //clear the selection and show msg if no candidate
            if(_candidates.Length == 0)
            {
                EditorGUILayout.HelpBox
                    ($"No {_propertyType.Name} property found.",
                    MessageType.None);
                spPropertyName.stringValue = null;
                return false;
            }

            // index of the current selection
            var index = Array.IndexOf(_candidates, spPropertyName.stringValue);

            //DropDown list
            EditorGUI.BeginChangeCheck();
            index = EditorGUILayout.Popup("Property", index, _candidates);
            if(EditorGUI.EndChangeCheck())
            {
                spPropertyName.stringValue = _candidates[index];
            }
            // Return true only if selection is valid
            return index >= 0;
        }
        #endregion
    }
}
