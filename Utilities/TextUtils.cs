//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;

namespace BookGen.Utilities
{
    internal static class TextUtils
    {
        public static string ReplaceTags(this string input, IEnumerable<KeyValuePair<string, string>> tags)
        {
            StringBuilder builder = new StringBuilder(input);
            foreach (var tag in tags)
            {
                builder.Replace($"[[{tag.Key}]]", tag.Value);
            }
            return builder.ToString();
        }

        public static string ReplaceTags(this string input, IEnumerable<string> values)
        {
            StringBuilder builder = new StringBuilder(input);
            int i = 0;
            foreach (var value in values)
            {
                builder.Replace($"[[{i}]]", value);
                ++i;
            }
            return builder.ToString();
        }
    }
}
