//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Shell.Shared;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("html2png")]
internal sealed class Html2PngCommand : AsyncCommand<Html2PngCommand.Html2PngArguments>
{
    internal sealed class Html2PngArguments : ArgumentsBase
    {
        [Switch("i", "input")]
        public string InputFile { get; set; }

        [Switch("o", "output")]
        public string OutputFile { get; set; }

        [Switch("w", "width")]
        public int Width { get; set; }

        [Switch("h", "height")]
        public int Height { get; set; }

        public Html2PngArguments()
        {
            InputFile = string.Empty;
            OutputFile = string.Empty;
            Width = 1920;
            Height = 1080;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            if (!context.FileSystem.FileExists(InputFile))
                return ValidationResult.Error($"File doesn't exist: {InputFile}");

            var extension = Path.GetExtension(InputFile).ToLower();

            if (extension != ".htm" && extension != ".html")
                return ValidationResult.Error("Input file isn't html");

            if (Width < 10)
                return ValidationResult.Error("Width must be at least 10px");

            if (Height < 10)
                return ValidationResult.Error("Height must be at least 10px");

            return ValidationResult.Ok();
        }

        public override void ModifyAfterValidation()
        {
            var extension = Path.GetExtension(InputFile).ToLower();

            if (extension != ".png")
                OutputFile = Path.ChangeExtension(OutputFile, ".png");
        }
    }

    private readonly BrowserInteract _browser;

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public Html2PngCommand(ILogger log)
    {
        _browser = new BrowserInteract(log);
    }

    public override async Task<int> Execute(Html2PngArguments arguments, string[] context)
    {
        bool result = await _browser.Html2Png(arguments.InputFile,
                                              arguments.OutputFile,
                                              arguments.Width,
                                              arguments.Height);

        return result ? ExitCodes.Succes : ExitCodes.GeneralError;
    }
}