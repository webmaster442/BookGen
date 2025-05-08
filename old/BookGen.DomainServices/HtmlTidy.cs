//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;

namespace BookGen.DomainServices
{
    public partial class HtmlTidy
    {
        private readonly string _tidyPath;
        private readonly Dictionary<string, string> _tagreplacements;
        private readonly ILogger _log;

        private const string TidyName = "tidy.exe";
        private const int TimeOut = 10;

        [GeneratedRegex("Tidy found ([0-9]+) warnings and ([0-9]+) errors!", RegexOptions.None, 5000)]
        private static partial Regex GetWarningAndErrorRegex();

        public HtmlTidy(ILogger log)
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
                { "abbr", "span" },
                { "acronym", "span" },
                { "address", "div" },
                { "article", "div" },
                { "aside", "div" },
                { "canvas", "div" },
                { "cite", "span" },
                { "dd", "span" },
                { "details", "div" },
                { "dfn", "span" },
                { "dl", "div" },
                { "dt", "span" },
                { "figcaption", "p" },
                { "figure", "div" },
                { "footer", "div" },
                { "header", "div" },
                { "kbd", "span" },
                { "nav", "div" },
                { "samp", "span" },
                { "section", "div" },
                { "var", "span" },
            };
            _log = log;
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
            var result = ProcessRunner.GetCommandOutput(_tidyPath, ["-asxhtml", "-utf8"], html, TimeOut);
            return GetResult(result);
        }

        public string XhtmlToHtml(string xhtml)
        {
            var result = ProcessRunner.GetCommandOutput(_tidyPath, ["-ashtml", "-utf8"], xhtml, TimeOut);
            return GetResult(result);
        }

        private string GetResult((string stdOut, string stdErr) result)
        {
            //tidy doesn't have option to only output errors, so manual extraction is done
            if (!string.IsNullOrEmpty(result.stdErr))
            {
                var matches = GetWarningAndErrorRegex().Match(result.stdErr);
                int errors = 0;
                int warnings = 0;
                if (matches.Groups.Count == 2)
                {
                    warnings = int.Parse(matches.Groups[1].Value);
                    errors = int.Parse(matches.Groups[2].Value);
                }

                if (warnings > 0)
                {
                    _log.LogWarning("Tidy found {warnings} warnings", warnings);
                }

                if (errors > 0)
                {
                    _log.LogCritical("Creating of XTML failed, Tidy found errors. See output file for details");
                    return $"<pre>{result.stdErr}</pre>";
                }
            }
            return result.stdOut;
        }
    }
}
