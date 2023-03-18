using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.Framework.Server;
using BookGen.Interfaces;
using Webmaster442.HttpServerFramework;

namespace BookGen.Commands
{
    [CommandName("preview")]
    internal class PreviewCommand : Command<BookGenArgumentBase>
    {
        private readonly ILog _log;
        private readonly IServerLog _serverlog;
        private readonly IAppSetting _appSettings;

        public PreviewCommand(ILog log, IServerLog serverLog, IAppSetting appSettings)
        {
            _log = log;
            _serverlog = serverLog;
            _appSettings = appSettings;
        }

        public override int Execute(BookGenArgumentBase arguments, string[] context)
        {
            const string url = "http://localhost:8082/";

            using (HttpServer? server = HttpServerFactory.CreateServerForPreview(_log, _serverlog, arguments.Directory))
            {
                server.Start();
                _log.Info("-------------------------------------------------");
                _log.Info("Test server running on: {0}", url);
                _log.Info("Serving from: {0}", arguments.Directory);

                if (_appSettings.AutoStartWebserver)
                {
                    UrlOpener.OpenUrl(url);
                }

                Console.WriteLine(GeneratorRunner.ExitString);
                Console.ReadLine();
                server.Stop();
            }
            return Constants.Succes;
        }
    }
}
