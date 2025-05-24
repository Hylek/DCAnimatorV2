using System;
using DC.Animator.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DC.Animator.Adapters
{
    public class ImageAdapter : IAnimatable
    {
        private readonly Image _image;

        public ImageAdapter(Image image) => _image = image;

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
                AnimatableProperty.Alpha => _image.color.a,
                AnimatableProperty.Color => _image.color,
                _ => throw new ArgumentException($"ImageAdapter does not support property {property}")
            };
        }

        public void SetValue(AnimatableProperty property, object value)
        {
            switch (property)
            {
                case AnimatableProperty.Alpha:
                    var colorAlpha = _image.color;
                    colorAlpha.a = (float)value;
                    _image.color = colorAlpha;
                    break;
                case AnimatableProperty.Color:
                    _image.color = (Color)value;
                    break;
                default:
                    throw new ArgumentException($"ImageAdapter does not support property {property}");
            }
        }

        public Type GetPropertyType(AnimatableProperty property)
        {
            return property switch
            {
                AnimatableProperty.Alpha => typeof(float),
                AnimatableProperty.Color => typeof(Color),
                _ => throw new ArgumentException($"ImageAdapter does not support property {property}")
            };
        }
    }
}