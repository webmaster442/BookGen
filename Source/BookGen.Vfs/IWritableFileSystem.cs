namespace BookGen.Vfs;

public interface IWritableFileSystem : IReadOnlyFileSystem
{
    Stream CreateStream(string path);
    void Delete(string path);
    Task WriteTextAsync(string path, string content);
    Task CopyTo(string path, string destination);
    void CreateDirectoryIfNotExist(string path);
}