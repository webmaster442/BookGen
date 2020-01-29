using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter
{
    public static class DocumentSelectors
    {
        private static string Cleanup(this string input)
        {
            return Regex.Replace(input, @"\s+", " ");
        }

        public static string GetSummary(XElement documentation, string memberpath)
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
    }
}
