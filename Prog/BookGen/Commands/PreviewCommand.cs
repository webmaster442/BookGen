//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Framework.Server;
using BookGen.Infrastructure;

using Webmaster442.HttpServerFramework;

namespace BookGen.Commands;

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

        _log.EnableVerboseLogingIfRequested(arguments);

        using (HttpServer? server = HttpServerFactory.CreateServerForPreview(_log, arguments.Directory))
        {
            server.Start();
            _log.Info("-------------------------------------------------");
            _log.Info("Test server running on: {0}", url);
            _log.Info($"To get QR code for another device visit: {url}/qrcodelink");
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
