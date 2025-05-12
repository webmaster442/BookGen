//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("html2pdf")]
internal sealed class Html2PdfCommand : AsyncCommand<Html2PdfCommand.Html2PdfArguments>
{
    internal sealed class Html2PdfArguments : ArgumentsBase
    {
        [Switch("i", "input")]
        public string InputFile { get; set; }

        [Switch("o", "output")]
        public string OutputFile { get; set; }

        public Html2PdfArguments()
        {
            InputFile = string.Empty;
            OutputFile = string.Empty;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            if (!context.FileSystem.FileExists(InputFile))
                return ValidationResult.Error($"File doesn't exist: {InputFile}");

            var extension = Path.GetExtension(InputFile).ToLower();

            if (extension != ".htm" && extension != ".html")
                return ValidationResult.Error("Input file isn't html");

            return ValidationResult.Ok();
        }

        public override void ModifyAfterValidation()
        {
            var extension = Path.GetExtension(InputFile).ToLower();

            if (extension != ".pdf")
                OutputFile = Path.ChangeExtension(OutputFile, ".pdf");
        }
    }

    private readonly BrowserInteract _browser;

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public Html2PdfCommand(ILogger log)
    {
        _browser = new BrowserInteract(log);
    }

    public override async Task<int> Execute(Html2PdfArguments arguments, string[] context)
    {
        bool result = await _browser.Html2Pdf(arguments.InputFile,
                                              arguments.OutputFile);

        return result ? ExitCodes.Succes : ExitCodes.GeneralError;
    }
}
