//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Lib.AppSettings;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown.RenderInterop;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Convert;

[CommandName("qrcode")]
[Description("Renders an url into a SVG QRCode image.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class QrCodeCommand : AsyncCommand<QrCodeCommand.Arguments>
{
    internal sealed class Arguments : ArgumentsBase
    {
        [Switch("o", "output", Required = true)]
        [Description("Output file.")]
        public string Output { get; set; }

        [Switch("d", "data", Required = true)]
        [Description("Url data to encode. Minimum 1 byte, Maximum 900 bytes")]
        public string Data { get; set; }

        public Arguments()
        {
            Output = string.Empty;
            Data = string.Empty;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();

            if (string.IsNullOrEmpty(Data))
                result.AddIssue("Data can't be empty");

            if (string.IsNullOrEmpty(Output))
                result.AddIssue("Output file must be specified");

            return result;
        }

        public override void ModifyAfterValidation()
        {
            Output = Path.ChangeExtension(Output, ".svg");
        }
    }


    private readonly ILogger _log;
    private readonly IWritableFileSystem _fileSystem;
    private readonly IAssetSource _assetSource;
    private readonly IProgramPathResolver _programPathResolver;

    public QrCodeCommand(ILogger log, IWritableFileSystem fileSystem, IProgramPathResolver programPathResolver, IAssetSource assetSource)
    {
        _log = log;
        _fileSystem = fileSystem;
        _programPathResolver = programPathResolver;
        _assetSource = assetSource;
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        using var render = IRenderInterop.CreateForSvg(_assetSource, _programPathResolver);

        ImageResult result = render.RenderQrCode(arguments.Data);

        await _fileSystem.WriteAllTextAsync(arguments.Output, result.Data);

        return ExitCodes.Success;
    }
}
