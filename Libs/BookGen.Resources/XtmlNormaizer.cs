//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Resources
{
    public static partial class XhtmlNormalizer
    {
        private static readonly Dictionary<string, string> replacements = new()
        {
            { "figure", "div" },
            { "article", "div" },
            { "details", "div" },
            { "footer", "div" },
            { "header", "div" },
            { "nav", "div" },
            { "section", "div" },
            { "figcaption", "p" }
        };

        [GeneratedRegex("(<script[^>]*>)([\\s\\S]*?)(</script>)")]
        private static partial Regex ScriptMather();

        [GeneratedRegex("(<style[^>]*>)([\\s\\S]*?)(</style>)")]
        private static partial Regex StyleMatcher();

        public static string NormalizeToXHTML(string input)
        {
            var buffer = new StringBuilder(input);


            foreach (KeyValuePair<string, string> elementToReplace in replacements)
            {
                //starting bracket
                buffer.Replace($"<{elementToReplace.Key}>", $"<{elementToReplace.Value}>");
                //end
                buffer.Replace($"</{elementToReplace.Key}>", $"</{elementToReplace.Value}>");
            }

            string? candidate = buffer.ToString();
            string? c2 = ScriptMather().Replace(candidate, "$1<![CDATA[$2]]>$3");
            return StyleMatcher().Replace(c2, "$1<![CDATA[$2]]>$3");
        }
    }
}
