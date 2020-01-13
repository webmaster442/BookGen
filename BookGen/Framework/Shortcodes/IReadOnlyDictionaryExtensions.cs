//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Framework.Shortcodes
{
    public static class IReadOnlyDictionaryExtensions
    {
        public static string GetArgumentOrThrow(this IReadOnlyDictionary<string, string> arguments, string argument)
        {
            var key = arguments.Keys.FirstOrDefault(k => string.Compare(k, argument, true) == 0);
            if (key == null)
                throw new ArgumentException($"{argument} was not found");

            return arguments[key];
        }

        public static string? GetArgument(this IReadOnlyDictionary<string, string> arguments, string argument)
        {
            var key = arguments.Keys.FirstOrDefault(k => string.Compare(k, argument, true) == 0);

            if (key == null)
                return null;

            return arguments[key];
        }
    }
}
