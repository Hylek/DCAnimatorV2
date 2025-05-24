using System;
using System.Collections.Generic;
using DC.Animator.Core;
using DCAnimator.Data;
using UnityEngine;

namespace DC.Animator.Data
{
    [CreateAssetMenu(fileName = "New Collection", menuName = "DCAnimator/Animation Collection")]
    public class AnimationCollection : ScriptableObject
    {
        public enum AnimationType
        {
            Fade,
            Move,
            Scale,
            Rotate,
            Color
        }

        public List<AnimationData> animations = new();

        public IAnimation CreateAnimation(IAnimatable target)
        {
            return animations.Count == 0 ? null : CreateAnimation(target, animations[0]);
        }

        public IAnimation CreateAnimation(IAnimatable target, string name)
        {
            foreach (var animData in animations)
                if (animData.name == name)
                    return CreateAnimation(target, animData);

            Debug.LogWarning($"Animation preset {name} not found");
            return null;
        }

        private static IAnimation CreateAnimation(IAnimatable target, AnimationData data)
        {
            var builder = new AnimationBuilder()
                .WithTarget(target)
                .WithDuration(data.duration)
                .WithDelay(data.delay)
                .WithEasing(data.easingType);

            IAnimation animation;

            switch (data.type)
            {
                case AnimationType.Fade:
                    animation = builder.AsFade()
                        .From(data.fromAlpha)
                        .To(data.toAlpha)
                        .Build();
                    break;

                case AnimationType.Move:
                    var moveBuilder = builder.AsMove()
                        .From(data.fromPosition)
                        .To(data.toPosition);

                    if (data.useLocalPosition)
                        moveBuilder.Local();
                    else if (data.useAnchoredPosition)
                        moveBuilder.Anchored();

                    animation = moveBuilder.Build();
                    break;

                case AnimationType.Scale:
                    animation = builder.AsScale()
                        .From(data.fromScale)
                        .To(data.toScale)
                        .Build();
                    break;

                case AnimationType.Rotate:
                    animation = builder.AsRotate()
                        .FromEuler(data.fromEulerAngles)
                        .ToEuler(data.toEulerAngles)
                        .Build();
                    break;

                case AnimationType.Color:
                    var colorBuilder = builder.AsColor()
                        .From(data.fromColor)
                        .To(data.toColor);

                    if (data.isTextColor)
                        colorBuilder.AsTextColor();

                    animation = colorBuilder.Build();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return animation;
        }
    }
}