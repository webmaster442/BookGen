//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Lib;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown.RenderInterop;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Convert;

[CommandName("convert diagram2svg")]
[Description("Renders a diagram definition file to an SVG file.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class Diagram2SvgCommand : AsyncCommand<Diagram2SvgCommand.Arguments>, IDisposable
{
    internal sealed class Arguments : ArgumentsBase
    {
        internal enum DiagramType
        {
            Unknown,
            Mermaid,
            Nomnoml,
        }

        [Switch("i", "input", Required = true)]
        [Description("Input file containing the diagram definition.")]
        public string InputFile { get; set; } = string.Empty;

        [Switch("o", "output", Required = true)]
        [Description("Output file where the rendered SVG will be saved.")]
        public string OutputFile { get; set; } = string.Empty;

        [Switch("t", "type", Required = true)]
        [Description("Type of the diagram to render. Can be Mermaid or Nomnoml.")]
        public DiagramType Type { get; set; } = DiagramType.Unknown;

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();

            if (string.IsNullOrEmpty(InputFile))
                result.AddIssue("Input file can't be empty");

            if (!context.FileSystem.FileExists(InputFile))
                result.AddIssue($"Input file '{InputFile}' does not exist");

            if (string.IsNullOrEmpty(OutputFile))
                result.AddIssue("Output file must be specified");

            if (Type == DiagramType.Unknown)
            {
                var extension = Path.GetExtension(InputFile).ToLower();
                switch (extension)
                {
                    case ".mmd":
                    case ".mermaid":
                    case ".nomnoml":
                        // Infer the diagram type from the file extension
                        break;
                    default:
                        result.AddIssue("Diagram type must be specified");
                        break;
                }
            }

            return result;
        }

        public override void ModifyAfterValidation()
        {
            var extension = Path.GetExtension(InputFile).ToLower();

            if (extension != ".svg")
                OutputFile = Path.ChangeExtension(OutputFile, ".svg");

            if (Type == DiagramType.Unknown)
            {
                Type = extension switch
                {
                    ".mmd" or ".mermaid" => DiagramType.Mermaid,
                    ".nomnoml" => DiagramType.Nomnoml,
                    _ => throw new System.Diagnostics.UnreachableException("Shouldn't happen"),
                };
            }
        }
    }

    private readonly ILogger _log;
    private readonly IWritableFileSystem _fileSystem;
    private readonly IProgramPathResolver _programPathResolver;
    private readonly IAssetSource _assetSource;
    private readonly IRenderInterop _renderInterop;

    public Diagram2SvgCommand(ILogger log, IWritableFileSystem fileSystem, IProgramPathResolver programPathResolver, IAssetSource assetSource)
    {
        _log = log;
        _fileSystem = fileSystem;
        _programPathResolver = programPathResolver;
        _assetSource = assetSource;
        _renderInterop = IRenderInterop.CreateForSvg(_assetSource, _programPathResolver);
    }

    public void Dispose()
    {
        _renderInterop.Dispose();
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        string inputContent = await _fileSystem.ReadAllTextAsync(arguments.InputFile);

        ImageResult result = arguments.Type switch
        {
            Arguments.DiagramType.Mermaid => _renderInterop.RenderMermaid(inputContent),
            Arguments.DiagramType.Nomnoml => _renderInterop.RenderNomnoml(inputContent),
            _ => throw new InvalidOperationException($"Unsupported diagram type: {arguments.Type}"),
        };

        await _fileSystem.WriteAllTextAsync(arguments.OutputFile, result.Data);

        return ExitCodes.Success;

    }
}
