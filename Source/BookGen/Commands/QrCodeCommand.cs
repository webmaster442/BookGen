//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

using Bookgen.Lib.AppSettings;
using Bookgen.Lib.Domain.IO.Legacy;
using Bookgen.Lib.Rendering.Images;
using Bookgen.Lib.Rendering.Markdown.RenderInterop;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("qrcode")]
internal sealed class QrCodeCommand : AsyncCommand<QrCodeCommand.QrCodeArguments>
{
    internal sealed class QrCodeArguments : ArgumentsBase
    {
        [Switch("o", "output")]
        public string Output { get; set; }

        [Switch("d", "data")]
        public string Data { get; set; }

        public QrCodeArguments()
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

    public override async Task<int> ExecuteAsync(QrCodeArguments arguments, IReadOnlyList<string> context)
    {
        using var render = IRenderInterop.CreateForSvg(_assetSource, _programPathResolver);

        ImageResult result = render.RenderQrCode(arguments.Data);

        await _fileSystem.WriteAllTextAsync(arguments.Output, result.Data);

        return ExitCodes.Success;
    }
}
