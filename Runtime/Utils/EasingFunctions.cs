using UnityEngine;

namespace DC.Animator.Utils
{
    public static class Easing
    {
        public delegate float Function(float t);

        public enum Type
        {
            Linear,
            EaseInSine,
            EaseOutSine,
            EaseInOutSine,
            EaseInQuad,
            EaseOutQuad,
            EaseInOutQuad,
            EaseInCubic,
            EaseOutCubic,
            EaseInOutCubic,
            EaseInQuart,
            EaseOutQuart,
            EaseInOutQuart,
            EaseInQuint,
            EaseOutQuint,
            EaseInOutQuint,
            EaseInExpo,
            EaseOutExpo,
            EaseInOutExpo,
            EaseInCirc,
            EaseOutCirc,
            EaseInOutCirc,
            EaseInBack,
            EaseOutBack,
            EaseInOutBack,
            EaseInElastic,
            EaseOutElastic,
            EaseInOutElastic,
            EaseInBounce,
            EaseOutBounce,
            EaseInOutBounce
        }

        private const float PI = Mathf.PI;
        private const float C1 = 1.70158f;
        private const float C2 = C1 * 1.525f;
        private const float C3 = C1 + 1f;
        private const float C4 = 2f * PI / 3f;
        private const float C5 = 2f * PI / 4.5f;

        public static Function GetEasingFunction(Type type)
        {
            return type switch
            {
                Type.Linear => Linear,
                Type.EaseInSine => EaseInSine,
                Type.EaseOutSine => EaseOutSine,
                Type.EaseInOutSine => EaseInOutSine,
                Type.EaseInQuad => EaseInQuad,
                Type.EaseOutQuad => EaseOutQuad,
                Type.EaseInOutQuad => EaseInOutQuad,
                Type.EaseInCubic => EaseInCubic,
                Type.EaseOutCubic => EaseOutCubic,
                Type.EaseInOutCubic => EaseInOutCubic,
                Type.EaseInQuart => EaseInQuart,
                Type.EaseOutQuart => EaseOutQuart,
                Type.EaseInOutQuart => EaseInOutQuart,
                Type.EaseInQuint => EaseInQuint,
                Type.EaseOutQuint => EaseOutQuint,
                Type.EaseInOutQuint => EaseInOutQuint,
                Type.EaseInExpo => EaseInExpo,
                Type.EaseOutExpo => EaseOutExpo,
                Type.EaseInOutExpo => EaseInOutExpo,
                Type.EaseInCirc => EaseInCirc,
                Type.EaseOutCirc => EaseOutCirc,
                Type.EaseInOutCirc => EaseInOutCirc,
                Type.EaseInBack => EaseInBack,
                Type.EaseOutBack => EaseOutBack,
                Type.EaseInOutBack => EaseInOutBack,
                Type.EaseInElastic => EaseInElastic,
                Type.EaseOutElastic => EaseOutElastic,
                Type.EaseInOutElastic => EaseInOutElastic,
                Type.EaseInBounce => EaseInBounce,
                Type.EaseOutBounce => EaseOutBounce,
                Type.EaseInOutBounce => EaseInOutBounce,
                _ => Linear
            };
        }

        public static float Linear(float t)
        {
            return t;
        }

        public static float EaseInSine(float t)
        {
            return 1f - Mathf.Cos(t * PI / 2f);
        }

        public static float EaseOutSine(float t)
        {
            return Mathf.Sin(t * PI / 2f);
        }

        public static float EaseInOutSine(float t)
        {
            return -(Mathf.Cos(PI * t) - 1f) / 2f;
        }

        public static float EaseInQuad(float t)
        {
            return t * t;
        }

        public static float EaseOutQuad(float t)
        {
            return 1f - (1f - t) * (1f - t);
        }

        public static float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        }

        public static float EaseInCubic(float t)
        {
            return t * t * t;
        }

        public static float EaseOutCubic(float t)
        {
            return 1f - Mathf.Pow(1f - t, 3f);
        }

        public static float EaseInOutCubic(float t)
        {
            return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
        }

        public static float EaseInQuart(float t)
        {
            return t * t * t * t;
        }

        public static float EaseOutQuart(float t)
        {
            return 1f - Mathf.Pow(1f - t, 4f);
        }

        public static float EaseInOutQuart(float t)
        {
            return t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4f) / 2f;
        }

        public static float EaseInQuint(float t)
        {
            return t * t * t * t * t;
        }

        public static float EaseOutQuint(float t)
        {
            return 1f - Mathf.Pow(1f - t, 5f);
        }

        public static float EaseInOutQuint(float t)
        {
            return t < 0.5f ? 16f * t * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 5f) / 2f;
        }

        public static float EaseInExpo(float t)
        {
            return t == 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);
        }

        public static float EaseOutExpo(float t)
        {
            return t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
        }

        public static float EaseInOutExpo(float t)
        {
            return t == 0f ? 0f :
                t == 1f ? 1f :
                t < 0.5f ? Mathf.Pow(2f, 20f * t - 10f) / 2f :
                (2f - Mathf.Pow(2f, -20f * t + 10f)) / 2f;
        }

        public static float EaseInCirc(float t)
        {
            return 1f - Mathf.Sqrt(1f - Mathf.Pow(t, 2f));
        }

        public static float EaseOutCirc(float t)
        {
            return Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f));
        }

        public static float EaseInOutCirc(float t)
        {
            return t < 0.5f
                ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * t, 2f))) / 2f
                : (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) / 2f;
        }

        public static float EaseInBack(float t)
        {
            return C3 * t * t * t - C1 * t * t;
        }

        public static float EaseOutBack(float t)
        {
            return 1f + C3 * Mathf.Pow(t - 1f, 3f) + C1 * Mathf.Pow(t - 1f, 2f);
        }

        public static float EaseInOutBack(float t)
        {
            return t < 0.5f
                ? Mathf.Pow(2f * t, 2f) * ((C2 + 1f) * 2f * t - C2) / 2f
                : (Mathf.Pow(2f * t - 2f, 2f) * ((C2 + 1f) * (t * 2f - 2f) + C2) + 2f) / 2f;
        }

        public static float EaseInElastic(float t)
        {
            return t switch
            {
                0f => 0f,
                1f => 1f,
                _ => -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * C4)
            };
        }

        public static float EaseOutElastic(float t)
        {
            return t switch
            {
                0f => 0f,
                1f => 1f,
                _ => Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * C4) + 1f
            };
        }

        public static float EaseInOutElastic(float t)
        {
            return t == 0f ? 0f :
                t == 1f ? 1f :
                t < 0.5f ? -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((20f * t - 11.125f) * C5)) / 2f :
                Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((20f * t - 11.125f) * C5) / 2f + 1f;
        }

        public static float EaseInBounce(float t)
        {
            return 1f - EaseOutBounce(1f - t);
        }

        public static float EaseOutBounce(float t)
        {
            switch (t)
            {
                case < 1f / 2.75f:
                    return 7.5625f * t * t;
                case < 2f / 2.75f:
                    t -= 1.5f / 2.75f;
                    return 7.5625f * t * t + 0.75f;
                case < 2.5f / 2.75f:
                    t -= 2.25f / 2.75f;
                    return 7.5625f * t * t + 0.9375f;
                default:
                    t -= 2.625f / 2.75f;
                    return 7.5625f * t * t + 0.984375f;
            }
        }

        public static float EaseInOutBounce(float t)
        {
            return t < 0.5f ? (1f - EaseOutBounce(1f - 2f * t)) / 2f : (1f + EaseOutBounce(2f * t - 1f)) / 2f;
        }
    }
}