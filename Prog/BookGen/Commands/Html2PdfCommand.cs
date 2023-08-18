using System.Diagnostics;

using BookGen.CommandArguments;

namespace BookGen.Commands;

[CommandName("html2pdf")]
internal class Html2PdfCommand : AsyncCommand<Html2PdfArguments>
{
    private readonly ILog _log;

    public Html2PdfCommand(ILog log)
    {
        _log = log;
    }

    public override async Task<int> Execute(Html2PdfArguments arguments, string[] context)
    {
        _log.Info("Rendering {0} to pdf..", arguments.InputFile.Filename);

        string[] edgeArguments = new[]
        {
            "--headless",
            "--disable-gpu",
            $"--print-to-pdf=\"{arguments.OutputFile}\"",
            "--disable-extensions",
            "--no-pdf-header-footer",
            "--disable-popup-blocking",
            "--run-all-compositor-stages-before-draw",
            "--disable-checker-imaging",
            $"file:///{arguments.InputFile}"
        };

        try
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "msedge.exe";
                process.StartInfo.Arguments = string.Join(' ', edgeArguments);
                process.StartInfo.UseShellExecute = true;
                process.Start();
                await process.WaitForExitAsync();
            }
            return Constants.Succes;
        }
        catch (Exception e)
        {
            _log.Critical(e);
            return Constants.GeneralError;
        }
    }

}
