using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.DomainServices.WebServices;
using System.Net;

namespace BookGen.Commands
{
    [CommandName("qrcode")]
    internal class QrCodeCommand : AsyncCommand<QrCodeArguments>
    {
        private readonly ILog _log;

        public QrCodeCommand(ILog log) 
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
                _log.Info("Downloading from {0}...", GoQrMeParams.ApiUrl);
                HttpStatusCode result = await client.DownloadToFile(uri, arguments.Output, _log);

                if (!BookGenHttpClient.IsSuccessfullRequest(result))
                {
                    _log.Warning("Download failed. Error: {0}", result);
                    return Constants.GeneralError;
                }

                return Constants.Succes;
            }

        }
    }
}
