//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

using Bookgen.Lib.JsInterop;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("qrcode")]
internal partial class QrCodeCommand : AsyncCommand<QrCodeCommand.QrCodeArguments>
{
    internal sealed partial class QrCodeArguments : ArgumentsBase
    {
        [GeneratedRegex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
        private static partial Regex ColorMatcher();

        [Switch("o", "output")]
        public string Output { get; set; }

        [Switch("c", "color")]
        public string Color { get; set; }

        [Switch("d", "data")]
        public string Data { get; set; }

        public QrCodeArguments()
        {
            Output = string.Empty;
            Data = string.Empty;
            Color = "#000000";
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();

            if (string.IsNullOrEmpty(Data))
                result.AddIssue("Data can't be empty");

            if (string.IsNullOrEmpty(Output))
                result.AddIssue("Output file must be specified");

            if (!ColorMatcher().IsMatch(Color))
                result.AddIssue("Color must be a valid hex color code, like #000000 or #FFF");

            return result;
        }
    }


    private readonly ILogger _log;
    private readonly IWritableFileSystem _fileSystem;
    private readonly IAssetSource _assetSource;

    public QrCodeCommand(ILogger log, IWritableFileSystem fileSystem, IAssetSource assetSource)
    {
        _log = log;
        _fileSystem = fileSystem;
        _assetSource = assetSource;
    }

    public override async Task<int> ExecuteAsync(QrCodeArguments arguments, IReadOnlyList<string> context)
    {
        using var qrCode = new QrCodeInterop(_assetSource);

        var svg = qrCode.GenerateQrCode(arguments.Data, arguments.Color);

        await _fileSystem.WriteAllTextAsync(arguments.Output, svg);

        return ExitCodes.Succes;
    }
}
