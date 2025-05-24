using DC.Animator.Core;
using UnityEngine;

namespace DC.Animator.AnimationTypeBuilders
{
    public class MoveAnimationBuilder : AnimationTypeBuilderBase
    {
        private Vector3 _fromPosition;
        private bool _hasFromPosition;
        private bool _hasToPosition;
        private bool _isAnchored;
        private bool _isLocal;
        private Vector3 _toPosition;

        public MoveAnimationBuilder(AnimationBuilder baseBuilder) : base(baseBuilder) { }

        public MoveAnimationBuilder From(Vector3 position)
        {
            _fromPosition = position;
            _hasFromPosition = true;
            return this;
        }

        public MoveAnimationBuilder To(Vector3 position)
        {
            _toPosition = position;
            _hasToPosition = true;
            return this;
        }

        public MoveAnimationBuilder FromCurrent()
        {
            _hasFromPosition = false;
            return this;
        }

        public MoveAnimationBuilder Local()
        {
            _isLocal = true;
            _isAnchored = false;
            return this;
        }

        public MoveAnimationBuilder Anchored()
        {
            _isAnchored = true;
            _isLocal = false;
            return this;
        }

        public override IAnimation Build()
        {
            var animation = BaseBuilder.Build();
            AnimatableProperty property;

            if (_isLocal)
                property = AnimatableProperty.LocalPosition;
            else if (_isAnchored)
                property = AnimatableProperty.AnchoredPosition;
            else
                property = AnimatableProperty.Position;

            if (!_hasFromPosition) _fromPosition = (Vector3)animation.GetValue(property);

            if (!_hasToPosition)
            {
                Debug.LogWarning("No target position specified for move animation");
                _toPosition = _fromPosition;
            }

            animation.SetProperty(property, _fromPosition, _toPosition);

            return animation;
        }
    }
}