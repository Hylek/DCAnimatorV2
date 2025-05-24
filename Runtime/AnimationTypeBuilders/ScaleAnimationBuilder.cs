using DC.Animator.Core;
using UnityEngine;

namespace DC.Animator.AnimationTypeBuilders
{
    public class ScaleAnimationBuilder : AnimationTypeBuilderBase
    {
        private Vector3 _fromScale = Vector3.one;
        private bool _hasFromScale;
        private Vector3 _toScale = Vector3.one;

        public ScaleAnimationBuilder(AnimationBuilder baseBuilder) : base(baseBuilder) { }

        public ScaleAnimationBuilder From(Vector3 scale)
        {
            _fromScale = scale;
            _hasFromScale = true;
            return this;
        }

        public ScaleAnimationBuilder From(float uniformScale)
        {
            _fromScale = new Vector3(uniformScale, uniformScale, uniformScale);
            _hasFromScale = true;
            return this;
        }

        public ScaleAnimationBuilder To(Vector3 scale)
        {
            _toScale = scale;
            return this;
        }

        public ScaleAnimationBuilder To(float uniformScale)
        {
            _toScale = new Vector3(uniformScale, uniformScale, uniformScale);
            return this;
        }

        public ScaleAnimationBuilder FromCurrent()
        {
            _hasFromScale = false;
            return this;
        }

        public override IAnimation Build()
        {
            var animation = BaseBuilder.Build();

            if (!_hasFromScale) _fromScale = (Vector3)animation.GetValue(AnimatableProperty.Scale);

            animation.SetProperty(AnimatableProperty.Scale, _fromScale, _toScale);

            return animation;
        }
    }
}