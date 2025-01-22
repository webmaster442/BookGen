using System.Numerics;

namespace Webmaster442.WindowsTerminal;

internal static class Extensions
{
    public static T Restrict<T>(this T value, T minValue, T maxValue) where T: IComparisonOperators<T, T, bool>
    {
        if (value < minValue)
            return minValue;

        if (value > maxValue)
            return maxValue;

        return value;
    }
}
