//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Lib.AppSettings;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown.RenderInterop;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Convert;

[CommandName("convert math2svg")]
[Description("Renders a single markdown file containing Tex formulas to svg files.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class Math2SvgCommand : AsyncCommand<Math2SvgCommand.Arguments>
{
    private readonly ILogger _log;
    private readonly IWritableFileSystem _fileSystem;
    private readonly IProgramPathResolver _programPathResolver;
    private readonly IAssetSource _assets;

    internal sealed class Arguments : ArgumentsBase
    {
        [Switch("f", "formula", Required = true)]
        [Description("The formula to render. It must be a valid Tex formula.")]
        public string Formula { get; set; } = string.Empty;

        [Switch("o", "output", Required = true)]
        [Description("The output file where the rendered SVG will be saved.")]
        public string OutputFile { get; set; } = string.Empty;

        [Switch("s", "scale", Required = false)]
        [Description("The scale factor for the rendered SVG. Must be between 0.1 and 40.")]
        public double Scale { get; set; } = 1.0;

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();

            if (string.IsNullOrEmpty(Formula))
                result.AddIssue("Formula can't be empty");

            if (string.IsNullOrEmpty(OutputFile))
                result.AddIssue("Output file must be specified");

            if (Scale <= 0.1 || Scale > 40)
                result.AddIssue("Scale must be bigger than 0.1 and maximum 40");

            return result;
        }

        public override void ModifyAfterValidation()
        {
            OutputFile = Path.ChangeExtension(OutputFile, ".svg");
        }
    }

    public Math2SvgCommand(ILogger log, IWritableFileSystem fileSystem, IProgramPathResolver programPathResolver, IAssetSource assetSource)
    {
        _log = log;
        _fileSystem = fileSystem;
        _programPathResolver = programPathResolver;
        _assets = assetSource;
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        using var render = IRenderInterop.CreateForSvg(_assets, _programPathResolver);

        ImageResult result = render.RenderLatex(arguments.Formula, arguments.Scale);

        await _fileSystem.WriteAllTextAsync(arguments.OutputFile, result.Data);

        return ExitCodes.Success;
    }
}
