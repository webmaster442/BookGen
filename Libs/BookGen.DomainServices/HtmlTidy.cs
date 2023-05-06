using System.Runtime.InteropServices;
using System.Text;

namespace BookGen.DomainServices
{
    public class HtmlTidy
    {
        private readonly string _tidyPath;
        private readonly Dictionary<string, string> _tagreplacements;
        private const string TidyName = "tidy.exe";
        private const int TimeOut = 10;

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
            return ProcessRunner.GetCommandOutput(_tidyPath,
                                                  new[] { "-asxhtml", "-utf8" },
                                                  html,
                                                  TimeOut);
        }

        public string XhtmlToHtml(string xhtml)
        {
            return ProcessRunner.GetCommandOutput(_tidyPath,
                                                  new[] { "-ashtml", "-utf8" },
                                                  xhtml,
                                                  TimeOut);
        }
    }
}
