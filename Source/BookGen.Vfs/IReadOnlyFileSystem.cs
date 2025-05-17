namespace BookGen.Vfs;

public interface IReadOnlyFileSystem
{
    string Scope { get; }
    TextReader OpenText(string path);
    Stream OpenStream(string path);
    DateTime GetLastModifiedUtc(string path);
    string ReadAllText(string path);
    bool FileExists(string path);
    bool DirectoryExists(string path);
    IEnumerable<string> GetFiles(string path, bool recursive);
    IEnumerable<string> GetDirectories(string path, bool recursive);
}
