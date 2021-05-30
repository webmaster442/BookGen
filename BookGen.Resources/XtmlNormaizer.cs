//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;

namespace BookGen.Resources
{
    public static class XhtmlNormalizer
    {
        private static Dictionary<string, string> replacements = new Dictionary<string, string>
        {
            { "figure", "div" },
            { "figcaption", "p" }
        };

        private static string[] selfclosing = new[]
        {
            "area",
            "base",
            "br",
            "col",
            "command",
            "embed",
            "hr",
            "img",
            "input",
            "keygen",
            "link",
            "menuitem",
            "meta",
            "param",
            "source",
            "track",
            "wbr"
        };

        public static string Html5ToXmlCompatible(string html)
        {
            StringBuilder buffer = new StringBuilder(html);

            foreach (var elementToReplace in selfclosing)
            {
                buffer.Replace($"<{elementToReplace}>", $"<{elementToReplace} />");
            }

            return buffer.ToString();
        }

        public static string Html5ToXhtml(string input)
        {
            StringBuilder buffer = new StringBuilder(input);

            foreach (var elementToReplace in replacements)
            {
                //starting bracket
                buffer.Replace($"<{elementToReplace.Key}>", $"<{elementToReplace.Value}>");
                //end
                buffer.Replace($"</{elementToReplace.Key}>", $"</{elementToReplace.Value}>");
            }

            return buffer.ToString();
        }
    }
}
