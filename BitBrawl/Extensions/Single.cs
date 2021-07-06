using System;

namespace BitBrawl.Extensions
{
    public static class SingleExtensions
    {
        public static bool IsApproximately(this float a, float b, float delta = 0.00390625f)
        {
            return Math.Abs(a - b) < delta;
        }

        /// <summary>
        /// Converts a float value as radians to degrees
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static float ToDegrees(this float radians)
        {
            return (float)(radians * 180 / Math.PI);
        }
    }
}
