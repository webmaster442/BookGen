using System.Diagnostics;
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

        private (string output, string error, int exitCode) RunTidy(string stdInput, string arguments)
        {
            (string output, string error, int exitCode) result = (string.Empty, string.Empty, 0);

            using (var process = new Process())
            {
                process.StartInfo.FileName = _tidyPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.StandardInputEncoding = Encoding.UTF8;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;

                bool startResult = process.Start();

                process.StandardInput.WriteLine("test");
                process.StandardInput.Flush();

                string output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();

                if (process.WaitForExit(TimeOut))
                {
                    result.output = output;
                    result.error = err;
                    result.exitCode = process.ExitCode;
                }
                else
                {
                    result.exitCode = int.MinValue;
                    result.output = output;
                    result.error = "timeout";
                    process.Kill();
                }
            }
            return result;
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
            var (output, _, exitCode) = RunTidy(html, "-asxhtml -utf8");
            if (exitCode == int.MinValue)
            {
                throw new InvalidOperationException("Timeout");
            }
            return output;
        }

        public string XhtmlToHtml(string xhtml)
        {
            var (output, _, exitCode) = RunTidy(xhtml, "-ashtml -utf8");
            if (exitCode == int.MinValue)
            {
                throw new InvalidOperationException("Timeout");
            }
            return output;
        }
    }
}
