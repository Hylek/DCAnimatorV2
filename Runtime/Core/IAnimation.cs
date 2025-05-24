using System;

namespace DC.Animator.Core
{
    public interface IAnimation
    {
        bool IsComplete { get; }
        bool IsPlaying { get; }
        
        void Start();
        void Pause();
        void Resume();
        void Stop();
        void Update(float deltaTime);
        
        void SetProperty(AnimatableProperty property, object fromValue, object toValue);
        object GetValue(AnimatableProperty property);
        
        event Action<IAnimation> OnStart;
        event Action<IAnimation> OnComplete;
        event Action<IAnimation> OnUpdate;
    }
}
