namespace BookGen.Launcher.ViewModels.FileBrowser;

internal sealed record class BookGenTask
{
    public required string Name { get; init; }
    public required string Command { get; init; }
}
