using System;

namespace Extensions
{
    public static class FloatExtensions
    {
        const float FloatTolerance = .0001f;

        public static bool EqualsFloat(this float floatOne, float floatTwo)
        {
            return Math.Abs(floatOne - floatTwo) <= FloatTolerance;
        }
    }
}