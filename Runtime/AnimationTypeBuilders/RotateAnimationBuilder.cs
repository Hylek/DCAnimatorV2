using DC.Animator.Core;
using UnityEngine;

namespace DC.Animator.AnimationTypeBuilders
{
    public class RotateAnimationBuilder : AnimationTypeBuilderBase
    {
        private Vector3 _fromEulerAngles;
        private Vector3 _toEulerAngles;
        private Quaternion _fromRotation;
        private Quaternion _toRotation;
        private bool _hasFromRotation;
        private bool _useEuler = true;
        
        public RotateAnimationBuilder(AnimationBuilder baseBuilder) : base(baseBuilder) { }
        
        public RotateAnimationBuilder FromEuler(Vector3 eulerAngles)
        {
            _fromEulerAngles = eulerAngles;
            _fromRotation = Quaternion.Euler(eulerAngles);
            _hasFromRotation = true;
            _useEuler = true;
            return this;
        }
        
        public RotateAnimationBuilder ToEuler(Vector3 eulerAngles)
        {
            _toEulerAngles = eulerAngles;
            _toRotation = Quaternion.Euler(eulerAngles);
            _useEuler = true;
            return this;
        }
        
        public RotateAnimationBuilder FromQuaternion(Quaternion rotation)
        {
            _fromRotation = rotation;
            _fromEulerAngles = rotation.eulerAngles;
            _hasFromRotation = true;
            _useEuler = false;
            return this;
        }
        
        public RotateAnimationBuilder ToQuaternion(Quaternion rotation)
        {
            _toRotation = rotation;
            _toEulerAngles = rotation.eulerAngles;
            _useEuler = false;
            return this;
        }
        
        public RotateAnimationBuilder FromCurrent()
        {
            _hasFromRotation = false;
            return this;
        }
        
        public override IAnimation Build()
        {
            var animation = BaseBuilder.Build();
            
            var property = _useEuler ? 
                AnimatableProperty.EulerAngles : 
                AnimatableProperty.Rotation;
            
            if (!_hasFromRotation)
            {
                if (_useEuler)
                {
                    _fromEulerAngles = (Vector3)animation.GetValue(AnimatableProperty.EulerAngles);
                    _fromRotation = Quaternion.Euler(_fromEulerAngles);
                }
                else
                {
                    _fromRotation = (Quaternion)animation.GetValue(AnimatableProperty.Rotation);
                    _fromEulerAngles = _fromRotation.eulerAngles;
                }
            }
            
            var fromValue = _useEuler ? (object)_fromEulerAngles : _fromRotation;
            var toValue = _useEuler ? (object)_toEulerAngles : _toRotation;
            
            animation.SetProperty(property, fromValue, toValue);
            
            return animation;
        }
    }
}