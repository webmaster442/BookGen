//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Vfs;

public sealed class FileSystemChangeEventArgs : EventArgs
{
    public required string FileName { get; init; }
    public string? NewFileName { get; init; }
    public required Change ChangeType { get; init; }

    public enum Change
    {
        Created,
        Changed,
        Deleted,
        Renamed
    }
}
