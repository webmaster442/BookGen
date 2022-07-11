//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace WpLoad.Infrastructure
{
    internal static class Extensions
    {
        public static bool TryGetArgument<T>(this IReadOnlyList<string> args, int index, [NotNullWhen(true)] out T? parsed) where T : IConvertible
        {
            if (index > args.Count || index < 0)
            {
                parsed = default;
                return false;
            }

            try
            {
                parsed = (T)Convert.ChangeType(args[index], typeof(T));
                return true;
            }
            catch (Exception)
            {
                parsed = default;
                return false;
            }
        }

    }
}
