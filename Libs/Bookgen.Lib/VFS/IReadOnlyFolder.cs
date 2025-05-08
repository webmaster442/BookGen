namespace Bookgen.Lib.VFS;

public interface IReadOnlyFolder
{
    string FullPath { get; }
    TextReader OpenText(string path);
    Stream OpenStream(string path);
    DateTime GetLastModifiedUtc(string path);
    string ReadText(string path);
    bool Exists(string path);
    IEnumerable<string> GetFiles(bool recursive);
    IEnumerable<string> GetDirectories(bool recursive);
}
