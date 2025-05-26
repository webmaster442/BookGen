namespace BookGen.Vfs;

public interface IWritableFileSystem : IReadOnlyFileSystem
{
    TextWriter CreateTextWriter(string path);
    Stream CreateWriteStream(string path);
    void WriteAllText(string path, string content);
    Task WriteAllTextAsync(string path, string content);
    void Delete(string path);
    Task CopyToAsync(string path, string destination);
    void MoveFile(string path, string destination);
    void CreateDirectoryIfNotExist(string path);
}