namespace BookGen.Infrastructure.Tools;

internal sealed class ToolInfo
{
    public required string Name { get; init; }
    public required string ApproximateSize { get; init; }
    public required string RepoOwner { get; init; }
    public required string RepoName { get; init; }
    public required string FolderName { get; init; }
}
