using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PlasticGui.WorkspaceWindow;
using WaveBinder;
using WaveBinderEditor;

public class ComponentUnitTest : Editor
{
    PropertyBinderEditor propertyBinderEditor;
    public List<Component> targetComponents = new List<Component>();
    // Property binders
    [SerializeReference] PropertyBinder[] _propertyBinders = null;
    public PropertyBinder[] propertyBinders
    {
        get => (PropertyBinder[])_propertyBinders.Clone();
        set => _propertyBinders = value;
    }

    private void OnEnable()
    {
      //  var finder = new PropertyFinder(serializedObject);
      //7  propertyBinderEditor = new PropertyBinderEditor(finder["_propertyBinders"]);
    }
}
