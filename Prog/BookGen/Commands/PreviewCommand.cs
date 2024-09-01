//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
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
    private readonly ILogger _log;
    private readonly IAppSetting _appSettings;
    private readonly ProgramInfo _programInfo;

    public PreviewCommand(ILogger log, IAppSetting appSettings, ProgramInfo programInfo)
    {
        _log = log;
        _appSettings = appSettings;
        _programInfo = programInfo;
    }

    public override int Execute(BookGenArgumentBase arguments, string[] context)
    {
        const string url = "http://localhost:8082/";

        _programInfo.EnableVerboseLogingIfRequested(arguments);

        using (HttpServer? server = HttpServerFactory.CreateServerForPreview(_log, _appSettings, arguments.Directory))
        {
            server.Start();
            _log.LogInformation("-------------------------------------------------");
            _log.LogInformation("Test server running on: {url}", url);
            _log.LogInformation("To get QR code for another device visit: {url}", $"{url}/qrcodelink");
            _log.LogInformation("Serving from: {dir}", arguments.Directory);

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
