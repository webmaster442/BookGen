//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Web;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("qrcode")]
internal class QrCodeCommand : AsyncCommand<QrCodeCommand.QrCodeArguments>
{
    internal sealed class QrCodeArguments : ArgumentsBase
    {
        [Switch("o", "output")]
        public string Output { get; set; }

        [Switch("s", "size")]
        public int Size { get; set; }

        [Switch("d", "data")]
        public string Data { get; set; }

        public QrCodeArguments()
        {
            Output = string.Empty;
            Size = 200;
            Data = string.Empty;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            ValidationResult result = new();
            if (string.IsNullOrEmpty(Data))
                result.AddIssue("Data can't be empty");

            if (Data?.Length < 1 || Data?.Length > 900)
                result.AddIssue("Data must be at least 1 chars and max 900 chars");

            if (string.IsNullOrEmpty(Output))
                result.AddIssue("Output can't be empty");

            if (Size < 10 || Size > 1000)
                result.AddIssue("Size must be bigger than 10px and maximum 1000 pixels");

            var extension = Path.GetExtension(Output);

            if (extension != ".png" && extension != ".svg")
                result.AddIssue("Output extension must be .png or .svg");

            return result;
        }
    }


    private readonly ILogger _log;
    private readonly IFileSystem _fileSystem;

    public QrCodeCommand(ILogger log, IFileSystem fileSystem)
    {
        _log = log;
        _fileSystem = fileSystem;
    }

    public override async Task<int> Execute(QrCodeArguments arguments, string[] context)
    {
        UrlParameterBuilder builder = new(GoQrMeParams.ApiUrl);
        builder.AddParameter(GoQrMeParams.DataParam, arguments.Data);
        builder.AddParameter(GoQrMeParams.SizeParam, $"{arguments.Size}x{arguments.Size}");
        builder.AddParameter(GoQrMeParams.FormatParam, Path.GetExtension(arguments.Output));

        var uri = builder.Build();

        using (var client = new BookGenHttpClient())
        {
            _log.LogInformation("Downloading from {url}...", GoQrMeParams.ApiUrl);

            using var output = _fileSystem.CreateStream(arguments.Output);

            HttpStatusCode result = await client.DownloadTo(uri, output);

            if (!BookGenHttpClient.IsSuccessfullRequest(result))
            {
                _log.LogWarning("Download failed. Error: {error}", result);
                return ExitCodes.GeneralError;
            }

            return ExitCodes.Succes;
        }
    }
}
