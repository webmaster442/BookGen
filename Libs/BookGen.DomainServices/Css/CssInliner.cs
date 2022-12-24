//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BookGen.DomainServices.Css
{
    public static partial class CssInliner
    {
        [GeneratedRegex("\\s*(?<rule>(?<selector>[^{}]+){(?<style>[^{}]+)})", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace)]
        private static partial Regex MathchStyles();

        private static List<Match> GetStyleMatches(XElement xhtmlDocument)
        {
            var styles = new List<Match>();

            var styleElements = xhtmlDocument.Descendants("style");
            foreach (var styleElement in styleElements)
            {
                var matches = MathchStyles().Matches(styleElement.Value);
                styles.AddRange(matches);
            }

            return styles;
        }

        private static XElement ParseXhtml(string xhtml)
        {
            return XElement.Parse(xhtml);
        }

        public static string Inline(string xhtml)
        {
            var xhtmlDocument = ParseXhtml(xhtml);
            var styles = GetStyleMatches(xhtmlDocument);

            foreach (var style in styles)
            {
                if (!style.Success)
                {
                    continue;
                }

                var cssSelector = style.Groups["selector"].Value.Trim();
                var xpathSelector = new CssToXpath().Transform(cssSelector);
                var cssStyle = style.Groups["style"].Value.Trim();

                foreach (var element in xhtmlDocument.XPathSelectElements(xpathSelector))
                {
                    var inlineStyle = element.Attribute("style");

                    var newInlineStyle = cssStyle + ";";
                    if (inlineStyle != null && !string.IsNullOrEmpty(inlineStyle.Value))
                    {
                        newInlineStyle += inlineStyle.Value;
                    }

                    element.SetAttributeValue("style", newInlineStyle
                        .Trim()
                        .NormalizeCharacter(';')
                        .NormalizeSpace());
                }
            }

            xhtmlDocument.Descendants("style").Remove();
            return xhtmlDocument.ToString();
        }
    }
}
