//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Terminal;
using BookGen.Tooldownloaders;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;
using Microsoft.IO;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("tools")]
internal sealed class ToolsCommand : AsyncCommand
{
    private readonly TooldownloaderBase[] _tooldownloaders;
    private readonly RecyclableMemoryStreamManager _memoryStreamManager;
    private readonly ILogger _logger;

    public ToolsCommand(IApiClient apiClient, ILogger logger)
    {
        _logger = logger;
        _memoryStreamManager = new RecyclableMemoryStreamManager(new RecyclableMemoryStreamManager.Options
        {
            ZeroOutBuffer = true,
            ThrowExceptionOnToArray = true,
        });
        _tooldownloaders =
        [
            new ChromaDownloader(apiClient, _memoryStreamManager, _logger),
            new CopyPartyDownloader(apiClient, _memoryStreamManager, _logger),
            new GithubDownloader(apiClient, _memoryStreamManager, _logger),
            new MicrosoftEditToolDownloader(apiClient, _memoryStreamManager, _logger),
            new PandocTooldownloader(apiClient, _memoryStreamManager, _logger),
        ];
    }

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public string ToSDisplayString(TooldownloaderBase tool)
        => $"{tool.ToolInfo.Name} (~{tool.ToolInfo.ApproximateSize})";

    public override async Task<int> ExecuteAsync(IReadOnlyList<string> context)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Tool installer"));

        var selectedItems = Terminal.SelectionMenu<TooldownloaderBase>(items: _tooldownloaders,
                                                                       title: "Select tools to download",
                                                                       instructions: "[grey](Press [blue]<space>[/] to toggle a tool for download, [green]<enter>[/] to accept)[/]",
                                                                       displaySelector: ToSDisplayString);


        foreach (var selected in selectedItems)
        {
            _logger.LogInformation("Installing {tool} ...", selected.ToolInfo.Name);
            var ui = new ToolDownloadUi();
            await selected.DownloadToolAsync(ui);
            ui.End();
        }

        return ExitCodes.Success;
    }
}
