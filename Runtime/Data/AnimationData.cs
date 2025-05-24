using System;
using DC.Animator.Data;
using DC.Animator.Utils;
using UnityEngine;

namespace DCAnimator.Data
{
    [Serializable]
    public class AnimationData
    {
        // Core properties
        public string name;
        public AnimationCollection.AnimationType type;
        public float duration = 0.5f;
        public float delay;
        public Easing.Type easingType = Easing.Type.EaseOutQuad;

        // Fade properties
        public float fromAlpha;
        public float toAlpha = 1f;

        // Move properties
        public Vector3 fromPosition;
        public Vector3 toPosition;
        public bool useLocalPosition = true;
        public bool useAnchoredPosition;

        // Scale properties
        public Vector3 fromScale = Vector3.one;
        public Vector3 toScale = Vector3.one;

        // Rotate properties
        public Vector3 fromEulerAngles;
        public Vector3 toEulerAngles;

        // Color properties
        public Color fromColor = Color.white;
        public Color toColor = Color.white;
        public bool isTextColor;
    }
}