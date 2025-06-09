//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.JsInterop;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("math2svg")]
internal class Math2SvgCommand : AsyncCommand<Math2SvgCommand.Math2SvgArguments>
{
    private readonly ILogger _log;
    private readonly IWritableFileSystem _fileSystem;
    private readonly IAssetSource _assets;

    public sealed class Math2SvgArguments : ArgumentsBase
    {
        [Switch("f", "formula")]
        public string Formula { get; set; } = string.Empty;

        [Switch("o", "output")]
        public string OutputFile { get; set; } = string.Empty;

        [Switch("s", "scale")]
        public double Scale { get; set; } = 1.0;

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();

            if (string.IsNullOrEmpty(Formula))
                result.AddIssue("Formula can't be empty");

            if (string.IsNullOrEmpty(OutputFile))
                result.AddIssue("Output file/directory must be specified");

            if (Scale <= 0.1 || Scale > 40)
                result.AddIssue("Scale must be bigger than 0.1 and maximum 40");

            return result;
        }

        public override void ModifyAfterValidation()
        {
            OutputFile = Path.ChangeExtension(OutputFile, ".svg");
        }
    }

    public Math2SvgCommand(ILogger log, IWritableFileSystem fileSystem, IAssetSource assetSource)
    {
        _log = log;
        _fileSystem = fileSystem;
        _assets = assetSource;
    }

    public override async Task<int> ExecuteAsync(Math2SvgArguments arguments, IReadOnlyList<string> context)
    {
        using var mathJax = new MathJaxInterop(_assets);
        string svg = mathJax.RenderLatexToSvg(arguments.Formula, arguments.Scale);

        await _fileSystem.WriteAllTextAsync(arguments.OutputFile, svg);

        return ExitCodes.Succes;
    }
}
