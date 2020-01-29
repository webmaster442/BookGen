//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter.Internals
{
    internal static class DocumentSelectors
    {
        private static string Cleanup(this string input)
        {
            return Regex.Replace(input, @"\s+", " ");
        }

        public static string GetPropertyOrTypeSummary(XElement documentation, string memberpath)
        {
            string summary = "";

            XElement node = documentation
                        .Descendants("members")
                        .Descendants("member")
                        .FirstOrDefault(x => x.Attribute("name").Value.EndsWith(memberpath));

            if (node != null)
            {
                summary = node.Descendants("summary")?.FirstOrDefault()?.Value.Cleanup() ?? "";
            }

            return summary;
        }

        public static string GetMethodSummary(XElement documentation, string methodname)
        {
            string summary = "";

            XElement node = documentation
                        .Descendants("members")
                        .Descendants("member")
                        .FirstOrDefault(x => x.Attribute("name").Value.StartsWith($"M:{methodname}"));

            if (node != null)
            {
                summary = node.Descendants("summary")?.FirstOrDefault()?.Value.Cleanup() ?? "";
            }

            return summary;
        }

        public static IEnumerable<(string name, string description)> GetMethodParamDescriptions(XElement documentation, string methodname)
        {
            XElement node = documentation
                        .Descendants("members")
                        .Descendants("member")
                        .FirstOrDefault(x => x.Attribute("name").Value.StartsWith($"M:{methodname}"));

            if (node != null)
            {
                foreach (var param in  node.Descendants("param"))
                {
                    var name = param.Attribute("name").Value;
                    var content = param.Value.Cleanup();

                    yield return (name, content);
                }
            }
        }
    }
}
