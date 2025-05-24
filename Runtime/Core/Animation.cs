using System;
using System.Collections.Generic;
using DC.Animator.Utils;
using UnityEngine;

namespace DC.Animator.Core
{
    public class Animation : IAnimation
    {
        private readonly Dictionary<AnimatableProperty, AnimationProperty> _properties = new();
        private float _currentTime;
        private float _delay;
        private float _delayTimer;
        private float _duration;
        private Easing.Function _easingFunction;
        private bool _hasStarted;

        private IAnimatable _target;

        public Animation() { }

        public Animation(IAnimatable target, float duration, float delay, Easing.Function easingFunction)
        {
            _target = target;
            _duration = Mathf.Max(duration, 0.001f); // Avoid division by zero
            _delay = delay;
            _easingFunction = easingFunction ?? Easing.Linear;

            Reset();
        }

        public bool IsComplete { get; private set; }
        public bool IsPlaying { get; private set; }

        public event Action<IAnimation> OnStart;
        public event Action<IAnimation> OnComplete;
        public event Action<IAnimation> OnUpdate;

        public void SetProperty(AnimatableProperty property, object fromValue, object toValue)
        {
            if (!_target.SupportsProperty(property))
            {
                Debug.LogWarning($"Target does not support property {property}");
                return;
            }

            var propertyType = _target.GetPropertyType(property);

            _properties[property] = new AnimationProperty
            {
                FromValue = fromValue,
                ToValue = toValue,
                PropertyType = propertyType
            };
        }

        public object GetValue(AnimatableProperty property)
        {
            if (_target != null && _target.SupportsProperty(property)) return _target.GetValue(property);
            throw new ArgumentException(
                $"Cannot get value for property {property}. Either target is null or property is not supported.");
        }

        public void Start()
        {
            if (IsPlaying) return;

            IsPlaying = true;
            IsComplete = false;
            _currentTime = 0;
            _delayTimer = 0;
            _hasStarted = false;

            foreach (var prop in _properties) _target.SetValue(prop.Key, prop.Value.FromValue);
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Resume()
        {
            if (!IsComplete) IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            
            foreach (var prop in _properties) _target.SetValue(prop.Key, prop.Value.ToValue);

            IsComplete = true;
            OnComplete?.Invoke(this);
        }

        public void Update(float deltaTime)
        {
            if (!IsPlaying || IsComplete) return;
            
            if (_delay > 0 && _delayTimer < _delay)
            {
                _delayTimer += deltaTime;
                return;
            }
            
            if (!_hasStarted)
            {
                _hasStarted = true;
                OnStart?.Invoke(this);
            }
            
            _currentTime += deltaTime;
            var normalizedTime = Mathf.Clamp01(_currentTime / _duration);
            var easedTime = _easingFunction(normalizedTime);
            
            foreach (var prop in _properties)
            {
                var interpolatedValue = InterpolateValue(
                    prop.Value.FromValue,
                    prop.Value.ToValue,
                    easedTime,
                    prop.Value.PropertyType
                );

                _target.SetValue(prop.Key, interpolatedValue);
            }

            OnUpdate?.Invoke(this);

            if (_currentTime >= _duration)
            {
                IsPlaying = false;
                IsComplete = true;
                OnComplete?.Invoke(this);
            }
        }

        public void Reset()
        {
            IsComplete = false;
            IsPlaying = false;
            _hasStarted = false;
            _currentTime = 0;
            _delayTimer = 0;
            _properties.Clear();

            OnStart = null;
            OnComplete = null;
            OnUpdate = null;
        }

        public void Initialize(IAnimatable target, float duration, float delay, Easing.Function easingFunction)
        {
            _target = target;
            _duration = Mathf.Max(duration, 0.001f);
            _delay = delay;
            _easingFunction = easingFunction ?? Easing.Linear;

            Reset();
        }

        private object InterpolateValue(object from, object to, float t, Type propertyType)
        {
            if (propertyType == typeof(float)) return Mathf.Lerp((float)from, (float)to, t);

            if (propertyType == typeof(Vector2)) return Vector2.Lerp((Vector2)from, (Vector2)to, t);

            if (propertyType == typeof(Vector3)) return Vector3.Lerp((Vector3)from, (Vector3)to, t);

            if (propertyType == typeof(Color)) return Color.Lerp((Color)from, (Color)to, t);

            if (propertyType == typeof(Quaternion)) return Quaternion.Slerp((Quaternion)from, (Quaternion)to, t);
            
            return t < 0.5f ? from : to;
        }

        private class AnimationProperty
        {
            public object FromValue;
            public Type PropertyType;
            public object ToValue;
        }
    }
}