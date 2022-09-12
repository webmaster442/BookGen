using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal static class PreviewHelper
    {
        public static bool IsMarkDown(string file)
        {
            return string.Equals(Path.GetExtension(file), ".md", StringComparison.OrdinalIgnoreCase);
        }

        public static async Task<(bool result, string output)> BookGenExport(string inputFile, bool raw, bool syntaxHighlight)
        {
            try
            {
                StringBuilder sb = new();
                sb.Append($"md2html -i \"{inputFile}\" -o con");
                if (raw)
                    sb.Append(" -r");
                if (!syntaxHighlight)
                    sb.Append(" -ns");

                string? arguments = sb.ToString();

                using var process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WorkingDirectory = AppContext.BaseDirectory;
                process.StartInfo.FileName = "bookgen.exe";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync();

                return (true, output);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
        }
    }
}
