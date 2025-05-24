using DC.Animator.Core;
using UnityEngine;

namespace DC.Animator.AnimationTypeBuilders
{
    public class ColorAnimationBuilder : AnimationTypeBuilderBase
    {
        private Color _fromColor = Color.white;
        private bool _hasFromColor;
        private bool _isTextColor;
        private Color _toColor = Color.white;

        public ColorAnimationBuilder(AnimationBuilder baseBuilder) : base(baseBuilder) { }

        public ColorAnimationBuilder From(Color color)
        {
            _fromColor = color;
            _hasFromColor = true;
            return this;
        }

        public ColorAnimationBuilder To(Color color)
        {
            _toColor = color;
            return this;
        }

        public ColorAnimationBuilder FromCurrent()
        {
            _hasFromColor = false;
            return this;
        }

        public ColorAnimationBuilder AsTextColor()
        {
            _isTextColor = true;
            return this;
        }

        public override IAnimation Build()
        {
            var animation = BaseBuilder.Build();

            var property = _isTextColor ? AnimatableProperty.TextColor : AnimatableProperty.Color;

            if (!_hasFromColor) _fromColor = (Color)animation.GetValue(property);

            animation.SetProperty(property, _fromColor, _toColor);

            return animation;
        }
    }
}