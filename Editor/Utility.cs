using UnityEditor;
using UnityEngine;

namespace WaveBinder.Editor
{   
    struct Label
    {
        GUIContent _guiContent;
        public static implicit operator GUIContent(Label label)
        {
            return label._guiContent;
        }

        public static implicit operator Label(string text)
        {
            return new Label { _guiContent = new GUIContent(text) };
        }
    }

    //this utility finds a property given a serialized object
    struct PropertyFinder
    {
        SerializedObject _so;
        public PropertyFinder(SerializedObject so)
        {
            _so = so;
        }

        public SerializedProperty this[string name] => _so.FindProperty(name);
        public SerializedProperty GetProperty(string name)
        {
            return _so.FindProperty(name);
        }
    }
    struct RelativePropertyFinder
    {
        SerializedProperty _sp;

        public RelativePropertyFinder(SerializedProperty sp)
        {
            _sp = sp;
        }

        public SerializedProperty this[string name] => _sp.FindPropertyRelative(name);
        public SerializedProperty GetProperty(string name)
        {
            return _sp.FindPropertyRelative(name);
        }

    }
    static class PropertyBinderTypeLabel<T>
    {
        static public GUIContent Content => _gui;
        static GUIContent _gui = new GUIContent
          (ObjectNames.NicifyVariableName(typeof(T).Name)
             .Replace("Property Binder", ""));
    }

    static class PropertyBinderNameUtil
    {
        public static string Shorten(SerializedProperty property)
        {
            string[] parts = property.managedReferenceFullTypename.Split('.');
            if (parts.Length > 0)
            {
                return ObjectNames.NicifyVariableName(parts[parts.Length - 1]);
            }

            return "Could not read property type";
        }
    }
}
