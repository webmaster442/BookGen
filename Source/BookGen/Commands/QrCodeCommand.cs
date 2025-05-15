//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.Infrastructure.Web;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("qrcode")]
internal class QrCodeCommand : AsyncCommand<QrCodeArguments>
{
    private readonly ILogger _log;

    public QrCodeCommand(ILogger log)
    {
        _log = log;
    }

    public override async Task<int> Execute(QrCodeArguments arguments, string[] context)
    {
        UrlParameterBuilder builder = new UrlParameterBuilder(GoQrMeParams.ApiUrl);
        builder.AddParameter(GoQrMeParams.DataParam, arguments.Data);
        builder.AddParameter(GoQrMeParams.SizeParam, $"{arguments.Size}x{arguments.Size}");
        builder.AddParameter(GoQrMeParams.FormatParam, arguments.Output.Extension);

        var uri = builder.Build();

        using (var client = new BookGenHttpClient())
        {
            _log.LogInformation("Downloading from {url}...", GoQrMeParams.ApiUrl);
            HttpStatusCode result = await client.DownloadToFile(uri, arguments.Output, _log);

            if (!BookGenHttpClient.IsSuccessfullRequest(result))
            {
                _log.LogWarning("Download failed. Error: {error}", result);
                return ExitCodes.GeneralError;
            }

            return ExitCodes.Succes;
        }

    }
}
