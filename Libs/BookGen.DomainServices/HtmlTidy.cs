using System.Diagnostics;

namespace BookGen.DomainServices
{
    internal class HtmlTidy
    {
        private readonly string _tidyPath;
        private const string TidyName = "tidy.exe";
        private const int TimeOut = 10_000;

        public HtmlTidy()
        {
            _tidyPath = Path.Combine(AppContext.BaseDirectory, TidyName);
            if (!File.Exists(_tidyPath))
                throw new InvalidOperationException("tidy can't be found");
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
                process.StartInfo.CreateNoWindow = true;

                process.Start();

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

        public string HtmlToXhtml(string html)
        {
            var (output, _, exitCode) = RunTidy(html, "-asxhtml -utf16");
            if (exitCode == int.MinValue)
            {
                throw new InvalidOperationException("Timeout");
            }
            return output;
        }

        public string XhtmlToHtml(string xhtml)
        {
            var (output, _, exitCode) = RunTidy(xhtml, "-ashtml -utf16");
            if (exitCode == int.MinValue)
            {
                throw new InvalidOperationException("Timeout");
            }
            return output;
        }
    }
}
