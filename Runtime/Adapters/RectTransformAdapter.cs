using System;
using DC.Animator.Core;
using UnityEngine;

namespace DC.Animator.Adapters
{
    public class RectTransformAdapter : IAnimatable
    {
        private readonly RectTransform _rectTransform;

        public RectTransformAdapter(RectTransform rectTransform) => _rectTransform = rectTransform;

        public bool SupportsProperty(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Position or AnimatableProperty.LocalPosition or AnimatableProperty.AnchoredPosition
                    or AnimatableProperty.Scale or AnimatableProperty.Rotation
                    or AnimatableProperty.EulerAngles => true,
                _ => false
            };
        }

        public object GetValue(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Position => _rectTransform.position,
                AnimatableProperty.LocalPosition => _rectTransform.localPosition,
                AnimatableProperty.AnchoredPosition => _rectTransform.anchoredPosition,
                AnimatableProperty.Scale => _rectTransform.localScale,
                AnimatableProperty.Rotation => _rectTransform.rotation,
                AnimatableProperty.EulerAngles => _rectTransform.eulerAngles,
                _ => throw new ArgumentException($"RectTransformAdapter does not support property {property}")
            };
        }

        public void SetValue(AnimatableProperty property, object value)
        {
            switch (property)
            {
                case AnimatableProperty.Position:
                    _rectTransform.position = (Vector3)value;
                    break;
                case AnimatableProperty.LocalPosition:
                    _rectTransform.localPosition = (Vector3)value;
                    break;
                case AnimatableProperty.AnchoredPosition:
                    _rectTransform.anchoredPosition = (Vector2)value;
                    break;
                case AnimatableProperty.Scale:
                    _rectTransform.localScale = (Vector3)value;
                    break;
                case AnimatableProperty.Rotation:
                    _rectTransform.rotation = (Quaternion)value;
                    break;
                case AnimatableProperty.EulerAngles:
                    _rectTransform.eulerAngles = (Vector3)value;
                    break;
                default:
                    throw new ArgumentException($"RectTransformAdapter does not support property {property}");
            }
        }

        public Type GetPropertyType(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Position or AnimatableProperty.LocalPosition or AnimatableProperty.Scale
                    or AnimatableProperty.EulerAngles => typeof(Vector3),
                AnimatableProperty.AnchoredPosition => typeof(Vector2),
                AnimatableProperty.Rotation => typeof(Quaternion),
                _ => throw new ArgumentException($"RectTransformAdapter does not support property {property}")
            };
        }
    }
}