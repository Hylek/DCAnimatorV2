using System;

namespace DC.Animator.Core
{
    public interface IAnimatable
    {
        bool SupportsProperty(AnimatableProperty property);
        object GetValue(AnimatableProperty property);
        void SetValue(AnimatableProperty property, object value);
        Type GetPropertyType(AnimatableProperty property);
    }
}