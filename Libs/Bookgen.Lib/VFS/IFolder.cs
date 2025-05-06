namespace Bookgen.Lib.VFS;

public interface IFolder
{
    string FullPath { get; }
    TextReader OpenText(string path);
    Stream OpenStream(string path);
    bool Exists(string path);
    DateTime GetLastModifiedUtc(string path);
    string GetText(string path);
    Stream CreateStream(string path);
}