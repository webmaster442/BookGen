using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace BookGen.DomainServices
{
    public partial class HtmlTidy
    {
        private readonly string _tidyPath;
        private readonly Dictionary<string, string> _tagreplacements;
        private const string TidyName = "tidy.exe";
        private const int TimeOut = 10;

        [GeneratedRegex("Tidy found ([0-9]+) warnings and ([0-9]+) errors!", RegexOptions.None, 5000)]
        private static partial Regex GetWarningAndErrorRegex();

        public HtmlTidy()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _tidyPath = Path.Combine(AppContext.BaseDirectory, TidyName);
                if (!File.Exists(_tidyPath))
                    throw new InvalidOperationException("tidy can't be found");
            }
            else
            {
                _tidyPath = "/usr/bin/tidy";
            }

            _tagreplacements = new Dictionary<string, string>()
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
        }

        public bool IsInstalled()
        {
            return File.Exists(_tidyPath);
        }

        public string ConvertHtml5TagsToXhtmlCompatible(string input)
        {
            var buffer = new StringBuilder(input);

            foreach (KeyValuePair<string, string> elementToReplace in _tagreplacements)
            {
                //starting bracket
                buffer.Replace($"<{elementToReplace.Key}>", $"<{elementToReplace.Value}>");
                //end
                buffer.Replace($"</{elementToReplace.Key}>", $"</{elementToReplace.Value}>");
            }

            return buffer.ToString();
        }

        public string HtmlToXhtml(string html)
        {
            var result = ProcessRunner.GetCommandOutput(_tidyPath, new[] { "-asxhtml", "-utf8" }, html, TimeOut);
            return GetResult(result);
        }

        public string XhtmlToHtml(string xhtml)
        {
            var result = ProcessRunner.GetCommandOutput(_tidyPath, new[] { "-ashtml", "-utf8"}, xhtml, TimeOut);
            return GetResult(result);
        }

        private static string GetResult((string stdOut, string stdErr) result)
        {
            //tidy doesn't have option to only output errors, so manual extraction is done
            if (!string.IsNullOrEmpty(result.stdErr))
            {
                var matches = GetWarningAndErrorRegex().Match(result.stdErr);
                int errors = 0;
                if (matches.Groups.Count > 2)
                {
                    errors = int.Parse(matches.Groups[2].Value);
                }

                if (errors > 0)
                    return $"<pre>{result.stdErr}</pre>";
            }
            return result.stdOut;
        }
    }
}
