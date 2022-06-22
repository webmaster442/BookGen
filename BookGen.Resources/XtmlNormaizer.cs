//-----------------------------------------------------------------------------
// (c) 2019-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.Resources
{
    public static class XhtmlNormalizer
    {
        private static Dictionary<string, string> replacements = new Dictionary<string, string>
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

        private static readonly Regex scriptMatcher = new(@"(<script[^>]*>)([\s\S]*?)(</script>)");
        private static readonly Regex styleMatcher = new(@"(<style[^>]*>)([\s\S]*?)(</style>)");

        public static string NormalizeToXHTML(string input)
        {
            StringBuilder buffer = new StringBuilder(input);


            foreach (var elementToReplace in replacements)
            {
                //starting bracket
                buffer.Replace($"<{elementToReplace.Key}>", $"<{elementToReplace.Value}>");
                //end
                buffer.Replace($"</{elementToReplace.Key}>", $"</{elementToReplace.Value}>");
            }

            var candidate = buffer.ToString();
            var c2 = scriptMatcher.Replace(candidate, "$1<![CDATA[$2]]>$3");
            return styleMatcher.Replace(c2, "$1<![CDATA[$2]]>$3");
        }
    }
}
