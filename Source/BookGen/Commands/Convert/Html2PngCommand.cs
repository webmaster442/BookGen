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

[CommandName("html2png")]
[Description("Converts a HTML file to a png using edges or chromes headless mode. The tool will use chrome, if it's installed, otherwise it will use edge. This command is only supported on Windows OS.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.GeneralError, "The command failed.")]
internal sealed class Html2PngCommand : AsyncCommand<Html2PngCommand.Html2PngArguments>
{
    internal sealed class Html2PngArguments : ArgumentsBase
    {
        [Switch("i", "input", Required = true)]
        [Description("Specifies the input HTML file.")]
        public string InputFile { get; set; }

        [Switch("o", "output", Required = true)]
        [Description("Specifies the output PNG file.")]
        public string OutputFile { get; set; }

        [Switch("w", "width", Required = false)]
        [Description("Specifies the width of the output PNG. If not given, the default is 1920.")]
        public int Width { get; set; }

        [Switch("h", "height", Required = false)]
        [Description("Specifies the height of the output PNG. If not given, the default is 1080.")]
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

            if (string.IsNullOrEmpty(OutputFile))
                return ValidationResult.Error("Output file not specified");

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

    public override async Task<int> ExecuteAsync(Html2PngArguments arguments, IReadOnlyList<string> context)
    {
        bool result = await _browser.Html2Png(arguments.InputFile,
                                              arguments.OutputFile,
                                              arguments.Width,
                                              arguments.Height);

        return result ? ExitCodes.Success : ExitCodes.GeneralError;
    }
}
