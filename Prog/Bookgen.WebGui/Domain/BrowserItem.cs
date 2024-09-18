namespace BookGen.WebGui.Domain;

public sealed class BrowserItem
{
    public required string Name { get; init; }
    public required string FullPath { get; init; }
    public required string Extension { get; init; }
    public required long Size { get; init; }
    public required DateTime LastModified { get; init; }
    public required bool IsDirectory { get; init; }
}
