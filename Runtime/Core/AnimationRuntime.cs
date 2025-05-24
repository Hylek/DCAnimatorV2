using System.Collections.Generic;
using DC.Animator.Utils;
using UnityEngine;

namespace DC.Animator.Core
{
    public class AnimationRuntime : MonoBehaviour
    {
        private static AnimationRuntime _instance;

        private readonly List<IAnimation> _activeAnimations = new();
        private readonly ObjectPool<Animation> _animationPool = new(() => new Animation());
        private readonly List<IAnimation> _pendingAdditions = new();
        private readonly List<IAnimation> _pendingRemovals = new();

        public static AnimationRuntime Instance
        {
            get
            {
                if (_instance) return _instance;

                var go = new GameObject("AnimationManager");
                _instance = go.AddComponent<AnimationRuntime>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (_pendingAdditions.Count > 0)
            {
                _activeAnimations.AddRange(_pendingAdditions);
                _pendingAdditions.Clear();
            }

            var deltaTime = Time.deltaTime;
            foreach (var animation in _activeAnimations)
            {
                animation.Update(deltaTime);

                if (animation.IsComplete) _pendingRemovals.Add(animation);
            }

            if (_pendingRemovals.Count > 0)
            {
                foreach (var animation in _pendingRemovals)
                {
                    _activeAnimations.Remove(animation);
                    ReturnToPool(animation);
                }

                _pendingRemovals.Clear();
            }
        }

        public void RegisterAnimation(IAnimation animation)
        {
            _pendingAdditions.Add(animation);
        }

        private void ReturnToPool(IAnimation animation)
        {
            if (animation is not Animation pooledAnimation) return;

            pooledAnimation.Reset();
            _animationPool.Return(pooledAnimation);
        }

        public Animation GetFromPool()
        {
            return _animationPool.Get();
        }

        public Animation CreateAnimation(IAnimatable target, float duration, float delay,
            Easing.Function easingFunction)
        {
            var animation = _animationPool.Get();
            animation.Initialize(target, duration, delay, easingFunction);
            return animation;
        }
    }
}