//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Convert;

[CommandName("html2pdf")]
[Description("Converts a HTML file to a png using edges or chromes headless mode. The tool will use chrome, if it's installed, otherwise it will use edge. This command is only supported on Windows OS.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.GeneralError, "The command failed.")]
internal sealed class Html2PdfCommand : AsyncCommand<Html2PdfCommand.Html2PdfArguments>
{
    internal sealed class Html2PdfArguments : ArgumentsBase
    {
        [Switch("i", "input", Required = true)]
        [Description("Specifies the input HTML file.")]
        public string InputFile { get; set; }

        [Switch("o", "output", Required = true)]
        [Description("Specifies the output PDF file.")]
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

            if (string.IsNullOrEmpty(OutputFile))
                return ValidationResult.Error("Output file not specified");

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

    public override async Task<int> ExecuteAsync(Html2PdfArguments arguments, IReadOnlyList<string> context)
    {
        bool result = await _browser.Html2Pdf(arguments.InputFile,
                                              arguments.OutputFile);

        return result ? ExitCodes.Success : ExitCodes.GeneralError;
    }
}
