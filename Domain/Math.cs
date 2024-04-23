namespace OWOPluginSimHub.Domain
{
    public static class Math
    {
        public static float Clamp(float value, float min, float max) => System.Math.Max(min, System.Math.Min(max, value));
    }
}