//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.WebGui.Domain;

public sealed class BrowserItem
{
    public required string Name { get; init; }
    public required string Id { get; init; }
    public required string Extension { get; init; }
    public required long Size { get; init; }
    public required DateTime LastModified { get; init; }
    public required bool IsDirectory { get; init; }
    public string ParentId { get; init; } = string.Empty;
}
