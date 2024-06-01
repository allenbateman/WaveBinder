using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace WaveBinder.Editor
{
    public class ComponentLister<T> where T : Component
    {
        public List<T> targetComponents = new List<T>();

        public void List()
        {
            Debug.Log("Hello");
            foreach (T component in targetComponents)
            {
                DisplayProperties(component);
            }
        }

        private void DisplayProperties(T component)
        {
            PropertyInfo[] properties = component.GetType().GetProperties();

            Debug.Log($"Properties of {component.GetType().Name}:");
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(Vector3))
                {
                    Vector3 value = (Vector3)property.GetValue(component);
                    Debug.Log($"{property.Name}: {value}");
                }
                else if (property.PropertyType == typeof(float))
                {
                    float value = (float)property.GetValue(component);
                    Debug.Log($"{property.Name}: {value}");
                }
                else if (property.PropertyType == typeof(Vector2))
                {
                    Vector2 value = (Vector2)property.GetValue(component);
                    Debug.Log($"{property.Name}: {value}");
                }
                else if (property.PropertyType == typeof(Color))
                {
                    Color value = (Color)property.GetValue(component);
                    Debug.Log($"{property.Name}: {value}");
                }
            }
        }
    }

}
