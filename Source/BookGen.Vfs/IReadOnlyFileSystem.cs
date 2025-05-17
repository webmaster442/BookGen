namespace BookGen.Vfs;

public interface IReadOnlyFileSystem
{
    string Scope { get; set; }
    TextReader OpenTextReader(string path);
    Stream OpenReadStream(string path);
    DateTime GetLastModifiedUtc(string path);
    string ReadAllText(string path);
    Task<string> ReadAllTextAsync(string path);
    bool FileExists(string path);
    bool DirectoryExists(string path);
    IEnumerable<string> GetFiles(string path, string filter, bool recursive);
    IEnumerable<string> GetDirectories(string path, bool recursive);
}
