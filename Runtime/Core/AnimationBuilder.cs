using System;
using System.Collections.Generic;
using DC.Animator.Adapters;
using DC.Animator.AnimationTypeBuilders;
using DC.Animator.Utils;
using UnityEngine;

namespace DC.Animator.Core
{
    public sealed class AnimationBuilder
    {
        private IAnimatable _target;
        private float _duration = 1f;
        private float _delay;
        private Easing.Type _easingType = Easing.Type.Linear;
        private Action<IAnimation> _onStart;
        private Action<IAnimation> _onComplete;
        private Action<IAnimation> _onUpdate;
        private static readonly Dictionary<Type, Func<object, IAnimatable>> AdapterFactories = new();

        static AnimationBuilder()
        {
            RegisterAdapter<UnityEngine.UI.Image>((img) => new ImageAdapter(img));
            RegisterAdapter<TMPro.TextMeshProUGUI>((text) => new TMPTextAdapter(text));
            RegisterAdapter<TMPro.TextMeshPro>((text) => new TMPTextAdapter(text));
            RegisterAdapter<SpriteRenderer>((sprite) => new SpriteAdapter(sprite));
            RegisterAdapter<Transform>((transform) => new TransformAdapter(transform));
            RegisterAdapter<RectTransform>((rectTransform) => new RectTransformAdapter(rectTransform));
        }

        public static void RegisterAdapter<T>(Func<T, IAnimatable> adapterFactory)
        {
            AdapterFactories[typeof(T)] = obj => adapterFactory((T)obj);
        }

        public static IAnimatable GetAdapter(object target)
        {
            var type = target.GetType();

            if (AdapterFactories.TryGetValue(type, out var factory))
                return factory(target);

            foreach (var entry in AdapterFactories)
            {
                if (entry.Key.IsAssignableFrom(type))
                    return entry.Value(target);
            }
            return null;
        }

        public static AnimationBuilder For<T>(T target) where T : UnityEngine.Object
        {
            var adapter = GetAdapter(target);
            if (adapter != null) return new AnimationBuilder().WithTarget(adapter);
            Debug.LogError($"No animation adapter registered for {typeof(T).Name}");
            return null;
        }

        public AnimationBuilder WithTarget(IAnimatable target)
        {
            _target = target;
            return this;
        }

        public AnimationBuilder WithDuration(float duration)
        {
            _duration = Mathf.Max(0.001f, duration);
            return this;
        }

        public AnimationBuilder WithDelay(float delay)
        {
            _delay = Mathf.Max(0, delay);
            return this;
        }

        public AnimationBuilder WithEasing(Easing.Type easingType)
        {
            _easingType = easingType;
            return this;
        }

        public AnimationBuilder OnStart(Action<IAnimation> callback)
        {
            _onStart = callback;
            return this;
        }

        public AnimationBuilder OnComplete(Action<IAnimation> callback)
        {
            _onComplete = callback;
            return this;
        }

        public AnimationBuilder OnUpdate(Action<IAnimation> callback)
        {
            _onUpdate = callback;
            return this;
        }

        public FadeAnimationBuilder AsFade() => new(this);

        public MoveAnimationBuilder AsMove() => new(this);

        public ScaleAnimationBuilder AsScale() => new(this);

        public RotateAnimationBuilder AsRotate() => new(this);

        public ColorAnimationBuilder AsColor() => new(this);

        public IAnimation Build()
        {
            var easingFunction = Easing.GetEasingFunction(_easingType);
            var animation = AnimationRuntime.Instance.CreateAnimation(_target, _duration, _delay, easingFunction);

            if (_onStart != null)
                animation.OnStart += _onStart;

            if (_onComplete != null)
                animation.OnComplete += _onComplete;

            if (_onUpdate != null)
                animation.OnUpdate += _onUpdate;

            return animation;
        }

        public IAnimation Start()
        {
            var animation = Build();
            AnimationRuntime.Instance.RegisterAnimation(animation);
            animation.Start();
            return animation;
        }
    }
}