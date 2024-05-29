using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaveBinderEditor
{
    sealed class ComponentSelector 
    {
        #region constructor
        ComponentSelector(GameObject gameObject)
        {
            // get all the names of the components of the game object into an array
            _candidates = gameObject?.GetComponents<Component>().Select(c => c.GetType().Name).ToArray();
        }
        string[] _candidates;
        #endregion

        #region static members
        // return a selector instance for a given target
        public static ComponentSelector GetInstance(SerializedProperty spTarget)
        {
            var component = spTarget.objectReferenceValue as Component;

            // case the target is not specified
            if( component == null)
                return _nullInstance;

            var gameObject  = component.gameObject;

            ComponentSelector selector;

            // try get the instance of a component
            if(_instances.TryGetValue(gameObject, out selector))
            {
                return selector;
            }

            //otherwise create a new instance
            selector = new ComponentSelector(gameObject);
            _instances[gameObject] = selector;

            return selector;
            
        }
        //clear any instance every time the inspector is initiated.
        public static void InvalidateCache() => _instances.Clear();
        static ComponentSelector _nullInstance = new ComponentSelector(null);
        static Dictionary<GameObject, ComponentSelector> _instances = new Dictionary<GameObject, ComponentSelector>();
        #endregion

        #region GUI implementation
        public bool ShowGUI(SerializedProperty spTarget)
        {
            if (_candidates == null)
                return false;

            var component = (Component)spTarget.objectReferenceValue;
            var gameObject = component.gameObject;

            //current slection
            var index = Array.IndexOf(_candidates, component.GetType().Name);

            //Component selection drop down
            EditorGUI.BeginChangeCheck();
            index = EditorGUILayout.Popup("Component", index, _candidates);
            if(EditorGUI.EndChangeCheck())
            {
                spTarget.objectReferenceValue = gameObject.GetComponent(_candidates[index]);
            }
            return true;
        }
        #endregion
    }
}
