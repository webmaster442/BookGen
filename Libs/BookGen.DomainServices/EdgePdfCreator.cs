using System.Diagnostics;

using BookGen.Interfaces;

namespace BookGen.DomainServices;

public static class EdgePdfCreator
{
    public static async Task CreatePdf(FsPath htmlFile, FsPath targetFile)
    {
        string[] arguments = new[]
        {
            "--headless",
            "--disable-gpu",
            $"--print-to-pdf=\"{targetFile}\"",
            "--disable-extensions",
            "--no-pdf-header-footer",
            "--disable-popup-blocking",
            "--run-all-compositor-stages-before-draw",
            "--disable-checker-imaging",
            $"file:///{htmlFile}"
        };

        using (var process = new Process())
        {
            process.StartInfo.FileName = "msedge.exe";
            process.StartInfo.Arguments = string.Join(' ', arguments);
            process.StartInfo.UseShellExecute = false;
            process.Start();
            await process.WaitForExitAsync();
        }
    }
}
