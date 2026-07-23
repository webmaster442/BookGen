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
