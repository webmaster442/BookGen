namespace BookGen.RenderEngine.Internals;

public sealed record class FunctionInfo
{
    public required string Name { get; init; }
    public required bool CanCacheResult { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyList<ArgumentInfo> ArgumentInfos { get; init; }
}
