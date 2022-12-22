using Medallion.Shell;
using System.Text;

namespace BookGen.DomainServices
{
    public class HtmlTidy
    {
        private readonly string _tidyPath;
        private readonly Dictionary<string, string> _tagreplacements;
        private const string TidyName = "tidy.exe";
        private const int TimeOut = 10_000;

        public HtmlTidy()
        {
            _tidyPath = Path.Combine(AppContext.BaseDirectory, TidyName);
            if (!File.Exists(_tidyPath))
                throw new InvalidOperationException("tidy can't be found");

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

        private string RunTidy(string input, params string[] arguments)
        {
            using var command = Command.Run(_tidyPath, arguments, (options) =>
            {
                options.Encoding(Encoding.UTF8);
                options.Timeout(TimeSpan.FromMilliseconds(TimeOut));
            }).RedirectFrom(input);

            command.Wait();

            return command.Result.StandardOutput;
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
            return RunTidy(html, "-asxhtml", "-utf8");
        }

        public string XhtmlToHtml(string xhtml)
        {
            return RunTidy(xhtml, "-ashtml", "-utf8");
        }
    }
}
