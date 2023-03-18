using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.Framework.Server;
using BookGen.Interfaces;

namespace BookGen.Commands
{
    [CommandName("preview")]
    internal class PreviewCommand : Command<BookGenArgumentBase>
    {
        private readonly ILog _log;
        private readonly IAppSetting _appSettings;

        public PreviewCommand(ILog log, IAppSetting appSettings)
        {
            _log = log;
            _appSettings = appSettings;
        }

        public override int Execute(BookGenArgumentBase arguments, string[] context)
        {
            const string url = "http://localhost:8082/";

            using (Webmaster442.HttpServerFramework.HttpServer? server = HttpServerFactory.CreateServerForPreview(CurrentState.Log, CurrentState.ServerLog, args.Directory))
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
