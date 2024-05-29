using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using WaveBinder;
using UnityEditor.Graphs;
using System.Diagnostics.Eventing.Reader;
using System;
using UnityEditor.ShaderGraph.Internal;
using System.Linq;

namespace WaveBinderEditor
{
    sealed class PropertyBinderEditor
    {
        public PropertyBinderEditor(SerializedProperty binders,SerializedProperty bands)
        {
            _binders = binders;
            _audioBands = bands;    
            ComponentSelector.InvalidateCache();
        }
        public void ShowGUI()
        {
            EditorGUILayout.Space();

            for (var i = 0; i < _binders.arraySize; i++)
                  ShowPropertyBinderEditor(i);

            CoreEditorUtils.DrawSplitter();
            EditorGUILayout.Space();

            // "Add Property Binder" button
            var rect = EditorGUILayout.GetControlRect();
            rect.x += (rect.width - 200) * 0.5f;
            rect.width = 200;

            if (GUI.Button(rect, "Add Property Binder"))
            {
                CreateNewPropertyBinderMenu().DropDown(rect);
            }

        }
        #region Private members
        SerializedProperty _binders;
        SerializedProperty _audioBands;
        static class Styles
        {
            public static Label Value0 = "Value at 0";
            public static Label Value1 = "Value at 1";
            public static Label MoveUp = "Move Up";
            public static Label MoveDown = "Move Down";
            public static Label Remove = "Remove";
            public static Label AudioBand = "AudioBand";
        }
        #endregion

        #region "Add Property Binder" button
        //create menu for selecting a porperty type to add
        GenericMenu CreateNewPropertyBinderMenu()
        {
            var menu = new GenericMenu();
            //add data types
            OnAddNewPropertyBinderItem<FloatPropertyBinder>(menu);
            OnAddNewPropertyBinderItem<Vector2PropertyBinder>(menu);
            OnAddNewPropertyBinderItem<Vector3PropertyBinder>(menu);
            OnAddNewPropertyBinderItem<ColorPropertyBinder>(menu);

            return menu;
        }

        void OnAddNewPropertyBinderItem<T>(GenericMenu menu) where T : new()
        {
            menu.AddItem(PropertyBinderTypeLabel<T>.Content, false, OnAddNewPropertyBinder<T>);
        }
        void OnAddNewPropertyBinder<T>() where T : new()
        {
            _binders.serializedObject.Update();

            var i = _binders.arraySize;
            _binders.InsertArrayElementAtIndex(i);
            _binders.GetArrayElementAtIndex(i).managedReferenceValue = new T();

            _binders.serializedObject.ApplyModifiedProperties();
        }
        #endregion
        #region PropertyBinder editor
        void ShowPropertyBinderEditor(int index)
        {
            var property = _binders.GetArrayElementAtIndex(index);
            var finder = new RelativePropertyFinder(property);

            // Header
            CoreEditorUtils.DrawSplitter();

            // create toggle to set enable or disable the property
            var toggle = CoreEditorUtils.DrawHeaderToggle(
             PropertyBinderNameUtil.Shorten(property)  , property, finder["Enabled"], pos => CreateHeaderContextMenu(index).DropDown(new Rect(pos,Vector2.zero)));

            if (!toggle) return;

            _binders.serializedObject.Update();
            
            EditorGUILayout.Space();
            //properties
            var target = finder["Target"]; // Find target var in the property binder generic class
            EditorGUILayout.PropertyField(target);

            if(ComponentSelector.GetInstance(target).ShowGUI(target) &&
                PropertySelector.GetInstance(target, finder["_propertyType"]).ShowGUI(finder["PropertyName"])
                && ShowAudioBandGUI(finder)) 
            {
               // EditorGUILayout.PropertyField(finder["AudioBand"],Styles.AudioBand);
                EditorGUILayout.PropertyField(finder["value0"], Styles.Value0);
                EditorGUILayout.PropertyField(finder["value1"], Styles.Value1);
            }

            _binders.serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();

        }
        //show the avaliable audio bands to select it and set it to recive the data 
        //of that audio band
        bool ShowAudioBandGUI(RelativePropertyFinder finder) 
        {
            if (!_audioBands.isArray)
            {
                Debug.Log("it is not an array");
                return false; 
            }


            string[] candidates = new string[_audioBands.arraySize];

            for(int i = 0; i< _audioBands.arraySize; i++) 
            {
                candidates[i] = i.ToString(); 
            }

            //clear the selection and show msg if no candidate
            if (candidates.Length == 0)
            {
                EditorGUILayout.HelpBox
                    ($"No audio bands set.",
                    MessageType.None);
                return false;
            }

            int currentIndex = finder["AudioBand"].intValue;
            EditorGUI.BeginChangeCheck();
            currentIndex = EditorGUILayout.Popup("Audio Band", currentIndex, candidates.ToArray());

            if(EditorGUI.EndChangeCheck())
            {
                finder["AudioBand"].intValue = currentIndex;
            }

            return currentIndex >= 0;         
        }

        #endregion

        #region PropertyBinder editor context menu
        GenericMenu CreateHeaderContextMenu(int index)
        {
            var menu = new GenericMenu();

            // move up a property in the list
            if(index == 0)
            {
                menu.AddDisabledItem(Styles.MoveUp);
            }
            else
            {
                menu.AddItem(Styles.MoveUp, false, () => OnMoveControl(index, index - 1));
            }

            // move down a property in the list

            if(index == _binders.arraySize - 1)
            {
                menu.AddDisabledItem(Styles.MoveDown);
            }
            else
            {
                menu.AddItem(Styles.MoveDown, false, () => OnMoveControl(index,_binders.arraySize + 1));
            }


            menu.AddSeparator(string.Empty);

            // remove item

            menu.AddItem(Styles.Remove,false, ()=> OnRemoveControl(index));

            return menu;
        }

        void OnMoveControl(int src, int dst)
        {
            _binders.serializedObject.Update();
            _binders.MoveArrayElement(src, dst);
            _binders.serializedObject.ApplyModifiedProperties();
        }
        void OnRemoveControl(int index)
        {
            _binders.serializedObject.Update();
            _binders.DeleteArrayElementAtIndex(index);
            _binders.serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}