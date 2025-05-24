using System;
using DC.Animator.Utils;

namespace DC.Animator.Core
{
    public abstract class AnimationTypeBuilderBase
    {
        protected AnimationBuilder BaseBuilder { get; }

        protected AnimationTypeBuilderBase(AnimationBuilder baseBuilder)
        {
            BaseBuilder = baseBuilder;
        }

        public AnimationTypeBuilderBase WithDuration(float duration)
        {
            BaseBuilder.WithDuration(duration);
            return this;
        }

        public AnimationTypeBuilderBase WithDelay(float delay)
        {
            BaseBuilder.WithDelay(delay);
            return this;
        }

        public AnimationTypeBuilderBase WithEasing(Easing.Type easingType)
        {
            BaseBuilder.WithEasing(easingType);
            return this;
        }

        public AnimationTypeBuilderBase OnStart(Action<IAnimation> callback)
        {
            BaseBuilder.OnStart(callback);
            return this;
        }

        public AnimationTypeBuilderBase OnComplete(Action<IAnimation> callback)
        {
            BaseBuilder.OnComplete(callback);
            return this;
        }

        public AnimationTypeBuilderBase OnUpdate(Action<IAnimation> callback)
        {
            BaseBuilder.OnUpdate(callback);
            return this;
        }

        public abstract IAnimation Build();

        public virtual IAnimation Start()
        {
            var animation = Build();
            AnimationRuntime.Instance.RegisterAnimation(animation);
            animation.Start();
            return animation;
        }
    }
}