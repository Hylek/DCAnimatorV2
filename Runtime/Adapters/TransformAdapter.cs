using System;
using DC.Animator.Core;
using UnityEngine;

namespace DC.Animator.Adapters
{
    public class TransformAdapter : IAnimatable
    {
        private readonly Transform _transform;
        
        public TransformAdapter(Transform transform) => _transform = transform;

        public bool SupportsProperty(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Position or AnimatableProperty.LocalPosition or AnimatableProperty.Scale
                    or AnimatableProperty.Rotation or AnimatableProperty.EulerAngles => true,
                _ => false
            };
        }
        
        public object GetValue(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Position => _transform.position,
                AnimatableProperty.LocalPosition => _transform.localPosition,
                AnimatableProperty.Scale => _transform.localScale,
                AnimatableProperty.Rotation => _transform.rotation,
                AnimatableProperty.EulerAngles => _transform.eulerAngles,
                _ => throw new ArgumentException($"TransformAdapter does not support property {property}")
            };
        }
        
        public void SetValue(AnimatableProperty property, object value)
        {
            switch (property)
            {
                case AnimatableProperty.Position:
                    _transform.position = (Vector3)value;
                    break;
                case AnimatableProperty.LocalPosition:
                    _transform.localPosition = (Vector3)value;
                    break;
                case AnimatableProperty.Scale:
                    _transform.localScale = (Vector3)value;
                    break;
                case AnimatableProperty.Rotation:
                    _transform.rotation = (Quaternion)value;
                    break;
                case AnimatableProperty.EulerAngles:
                    _transform.eulerAngles = (Vector3)value;
                    break;
                default:
                    throw new ArgumentException($"TransformAdapter does not support property {property}");
            }
        }
        
        public Type GetPropertyType(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Position or AnimatableProperty.LocalPosition or AnimatableProperty.Scale
                    or AnimatableProperty.EulerAngles => typeof(Vector3),
                AnimatableProperty.Rotation => typeof(Quaternion),
                _ => throw new ArgumentException($"TransformAdapter does not support property {property}")
            };
        }
    }
}