using System;
using DC.Animator.Core;
using TMPro;
using UnityEngine;

namespace DC.Animator.Adapters
{
    public class TMPTextAdapter : IAnimatable
    {
        private readonly TMP_Text _text;
        
        public TMPTextAdapter(TMP_Text text) => _text = text;

        public bool SupportsProperty(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Alpha or AnimatableProperty.Color or AnimatableProperty.TextColor => true,
                _ => false
            };
        }
        
        public object GetValue(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Alpha => _text.alpha,
                AnimatableProperty.Color or AnimatableProperty.TextColor => _text.color,
                _ => throw new ArgumentException($"TextMeshProAdapter does not support property {property}")
            };
        }
        
        public void SetValue(AnimatableProperty property, object value)
        {
            switch (property)
            {
                case AnimatableProperty.Alpha:
                    _text.alpha = (float)value;
                    break;
                case AnimatableProperty.Color:
                case AnimatableProperty.TextColor:
                    _text.color = (Color)value;
                    break;
                default:
                    throw new ArgumentException($"TextMeshProAdapter does not support property {property}");
            }
        }
        
        public Type GetPropertyType(AnimatableProperty property)
        {
            switch (property)
            {
                case AnimatableProperty.Alpha:
                    return typeof(float);
                case AnimatableProperty.Color:
                case AnimatableProperty.TextColor:
                    return typeof(Color);
                default:
                    throw new ArgumentException($"TextMeshProAdapter does not support property {property}");
            }
        }
    }
}