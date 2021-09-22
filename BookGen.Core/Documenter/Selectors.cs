//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BookGen.Core.Documenter
{
    internal static class Selectors
    {
        private static string Cleanup(this string input)
        {
            return Regex.Replace(input, @"\s+", " ");
        }

        public static string GetPropertyOrTypeSummary(XElement documentation, string memberpath)
        {
            string summary = "";

            XElement? node = documentation
                        .Descendants(XmlTags.Members)
                        .Descendants(XmlTags.Member)
                        .FirstOrDefault(x => x?.Attribute(XmlTags.Name)?.Value?.EndsWith(memberpath) ?? false);

            if (node != null)
            {
                summary = node.Descendants(XmlTags.Summary)?.FirstOrDefault()?.Value.Cleanup() ?? "";
            }

            return summary;
        }

        public static string GetMethodSummary(XElement documentation, string methodname)
        {
            string summary = "";

            XElement? node = documentation
                        .Descendants(XmlTags.Members)
                        .Descendants(XmlTags.Member)
                        .FirstOrDefault(x => x?.Attribute(XmlTags.Name)?.Value?.StartsWith($"M:{methodname}") ?? false);

            if (node != null)
            {
                summary = node.Descendants(XmlTags.Summary)?.FirstOrDefault()?.Value.Cleanup() ?? "";
            }

            return summary;
        }

        public static IEnumerable<(string name, string description)> GetMethodParamDescriptions(XElement documentation, string methodname)
        {
            XElement? node = documentation
                        .Descendants(XmlTags.Members)
                        .Descendants(XmlTags.Member)
                        .FirstOrDefault(x => x?.Attribute(XmlTags.Name)?.Value?.StartsWith($"M:{methodname}") ?? false);

            if (node != null)
            {
                foreach (var param in node.Descendants(XmlTags.Param))
                {
                    var name = param?.Attribute(XmlTags.Name)?.Value ?? string.Empty;
                    var content = param?.Value?.Cleanup() ?? string.Empty;

                    yield return (name, content);
                }
            }
        }
    }
}
