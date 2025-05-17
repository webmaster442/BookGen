
using BookGen.Cli;

namespace Bookgen.Lib.VFS;

public class FileSystemFolder : IReadOnlyFolder
{
    private readonly IFileSystem _fileSystem;

    public FileSystemFolder(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public string FullPath => string.Empty;

    public bool Exists(string path)
        => _fileSystem.FileExists(path);

    public IEnumerable<string> GetDirectories(bool recursive)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetFiles(bool recursive)
    {
        throw new NotImplementedException();
    }

    public DateTime GetLastModifiedUtc(string path)
    {
        throw new NotImplementedException();
    }

    public Stream OpenStream(string path)
    {
        throw new NotImplementedException();
    }

    public TextReader OpenText(string path)
    {
        throw new NotImplementedException();
    }

    public string ReadText(string path)
        => _fileSystem.ReadAllText(path);
}
