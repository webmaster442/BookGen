namespace Bookgen.Lib.VFS;

public interface IReadOnlyFolder
{
    string FullPath { get; }
    TextReader OpenText(string path);
    Stream OpenStream(string path);
    DateTime GetLastModifiedUtc(string path);
    string GetText(string path);
    bool Exists(string path);
}