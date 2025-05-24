using DC.Animator.Core;
using UnityEngine;

namespace DC.Animator.AnimationTypeBuilders
{
    public class FadeAnimationBuilder : AnimationTypeBuilderBase
    {
        private float _fromAlpha;
        private float _toAlpha = 1f;

        public FadeAnimationBuilder(AnimationBuilder baseBuilder) : base(baseBuilder) { }

        public FadeAnimationBuilder From(float alpha)
        {
            _fromAlpha = Mathf.Clamp01(alpha);
            return this;
        }

        public FadeAnimationBuilder To(float alpha)
        {
            _toAlpha = Mathf.Clamp01(alpha);
            return this;
        }

        public override IAnimation Build()
        {
            var animation = BaseBuilder.Build();
            animation.SetProperty(AnimatableProperty.Alpha, _fromAlpha, _toAlpha);
            return animation;
        }
    }
}