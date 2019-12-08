//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using System.Collections.Generic;
using System.Text;

namespace Bookgen.Template
{
    public static class XhtmlNormalizer
    {
        private static Dictionary<string, string> replacements = new Dictionary<string, string>
        {
            { "figure", "div" },
            { "figcaption", "p" }
        };

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

            return buffer.ToString();
        }
    }
}
