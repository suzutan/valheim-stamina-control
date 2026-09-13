namespace StaminaControl
{
    internal static class MultiplierMath
    {
        internal static float Normalize(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value)) return 1f;
            if (value < 0f) return 0f;
            return value > 20f ? 20f : value;
        }

        internal static float ScalePositive(float amount, float multiplier)
        {
            // Negative amounts may be refunds: do not convert or multiply them.
            return amount > 0f ? amount * multiplier : amount;
        }
    }
}
