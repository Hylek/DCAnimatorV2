using System;
using DC.Animator.Core;
using UnityEngine;

namespace DC.Animator.Adapters
{
    public class SpriteAdapter : IAnimatable
    {
        private readonly SpriteRenderer _spriteRenderer;

        public SpriteAdapter(SpriteRenderer spriteRenderer) => _spriteRenderer = spriteRenderer;

        public bool SupportsProperty(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Alpha or AnimatableProperty.Color => true,
                _ => false
            };
        }

        public object GetValue(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Alpha => _spriteRenderer.color.a,
                AnimatableProperty.Color => _spriteRenderer.color,
                _ => throw new ArgumentException($"SpriteRendererAdapter does not support property {property}")
            };
        }

        public void SetValue(AnimatableProperty property, object value)
        {
            switch (property)
            {
                case AnimatableProperty.Alpha:
                    var colorAlpha = _spriteRenderer.color;
                    colorAlpha.a = (float)value;
                    _spriteRenderer.color = colorAlpha;
                    break;
                case AnimatableProperty.Color:
                    _spriteRenderer.color = (Color)value;
                    break;
                default:
                    throw new ArgumentException($"SpriteRendererAdapter does not support property {property}");
            }
        }

        public Type GetPropertyType(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Alpha => typeof(float),
                AnimatableProperty.Color => typeof(Color),
                _ => throw new ArgumentException($"SpriteRendererAdapter does not support property {property}")
            };
        }
    }
}