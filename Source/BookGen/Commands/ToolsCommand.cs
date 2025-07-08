using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Terminal;
using BookGen.Tooldownloaders;
using BookGen.Vfs;

using Microsoft.IO;

namespace BookGen.Commands;

[CommandName("tools")]
internal sealed class ToolsCommand : AsyncCommand
{
    private readonly TooldownloaderBase[] _tooldownloaders;
    private readonly RecyclableMemoryStreamManager _memoryStreamManager;

    public ToolsCommand(IApiClient apiClient)
    {
        _memoryStreamManager = new RecyclableMemoryStreamManager(new RecyclableMemoryStreamManager.Options
        {
            ZeroOutBuffer = true,
            ThrowExceptionOnToArray = true,
        });
        _tooldownloaders =
        [
            new PandocTooldownloader(apiClient, _memoryStreamManager),
            new MicrosoftEditToolDownloader(apiClient, _memoryStreamManager),
        ];
    }

    public override SupportedOs SupportedOs => SupportedOs.Windows;

    public override async Task<int> ExecuteAsync(IReadOnlyList<string> context)
    {
        var selectedItems = Terminal.SelectionMenu<TooldownloaderBase>(items: _tooldownloaders,
                                                                       title: "Tool downloader",
                                                                       instructions: "[grey](Press [blue]<space>[/] to toggle a tool for download, [green]<enter>[/] to accept)[/]",
                                                                       displaySelector: t => t.ToolName);


        foreach (var selected in selectedItems)
        {
            var ui = new ToolDownloadUi();
            await selected.DownloadToolAsync(ui);
            ui.End();
        }

        return ExitCodes.Succes;
    }
}
