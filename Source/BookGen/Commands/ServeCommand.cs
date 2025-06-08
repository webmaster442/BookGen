//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Http;

using BookGen.Cli;
using BookGen.Cli.Annotations;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("serve")]
internal class ServeCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly ILogger _log;

    public ServeCommand(ILogger log)
    {
        _log = log;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        //runner is responsible for disposing the server
        await using (var runner = new ConsoleHttpServerRunner(ServerFactory.CreateServerForDirectoryHosting(arguments.Directory)))
        {
            var serverurls = string.Join(' ', runner.Server.GetListenUrls());
            var qrcodes = string.Join(' ', runner.Server.GetListenUrls().Select(x => $"{x}/qrcodelink"));

            _log.LogInformation("Serving: {directory}", arguments.Directory);
            _log.LogInformation("Server running on {urls}", serverurls);
            _log.LogInformation("To get QR code for another device visit: {qrcodes}", qrcodes);

            await runner.RunServer();
        }
#pragma warning restore CA2000 // Dispose objects before losing scope

        return ExitCodes.Succes;
    }
}
