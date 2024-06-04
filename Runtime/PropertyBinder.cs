using UnityEngine;
using UnityEngine.Events;

namespace WaveBinder.Runtime
{
    [System.Serializable]
    public abstract class PropertyBinder
    {
        //to turn on/off
        public bool Enabled = true;
        //audio level passed to the property, setter only
        public float Level { set { if (Enabled) OnSetLevel(value); } }
        //audio band index 
        public int AudioBand = 0;
        // Binder implementation
        protected abstract void OnSetLevel(float level);
    }

    public abstract class GenericPropertyBinder<T> : PropertyBinder
    {
        //serailized target property information
        public Component Target;
        public string PropertyName;

        // only used in editor, never use in runtime
        [SerializeField, HideInInspector]
        string _propertyType = typeof(T).AssemblyQualifiedName;

        //target property setter
        protected T TargetProperty { set => SetTargetProperty(value); }

        UnityAction<T> _setterCache;
        void SetTargetProperty(T value)
        {
            if (_setterCache == null)
            {
                if (Target == null) return;
                if (string.IsNullOrEmpty(PropertyName)) return;

                _setterCache = (UnityAction<T>)System.Delegate.CreateDelegate(typeof(UnityAction<T>), Target, "set_" + PropertyName);

            }
            _setterCache(value);
        }
    }
    //binder for float porperties
    public sealed class FloatPropertyBinder : GenericPropertyBinder<float>
    {
        public float value0 = 0;
        public float value1 = 1;
        protected override void OnSetLevel(float level)
        {
            TargetProperty = Mathf.Lerp(value0, value1, level);
        }
    }
    //binder for vector2 porperties
    public sealed class Vector2PropertyBinder : GenericPropertyBinder<Vector2>
    {
        public Vector2 value0 = Vector2.zero;
        public Vector2 value1 = Vector2.one;

        protected override void OnSetLevel(float level)
        {
            TargetProperty = Vector2.Lerp(value0, value1, level);
        }
    }
    //binder for vector3 porperties
    public sealed class Vector3PropertyBinder : GenericPropertyBinder<Vector3>
    {
        public Vector3 value0 = Vector3.zero;
        public Vector3 value1 = Vector3.one;
        protected override void OnSetLevel(float level)
        {
            TargetProperty = Vector3.Lerp(value0,value1, level);
        }
    }
    //binder for vecto4 porperties
    public sealed class Vector4PropertyBinder : GenericPropertyBinder<Vector4>
    {
        public Vector4 value0 = Vector4.zero;
        public Vector4 value1 = Vector4.one;

        protected override void OnSetLevel(float level)
        {
            TargetProperty = Vector4.Lerp(value0, value1, level);
        }
    }
    //binder for color porperties
    public sealed class ColorPropertyBinder : GenericPropertyBinder<Color>
    {
        [ColorUsage(true, true)]
        public Color value0 = Color.black;
        [ColorUsage(true, true)]
        public Color value1 = Color.white;

        protected override void OnSetLevel(float level)
        {
            TargetProperty = Color.Lerp(value0,value1, level);
        }
    }
}
