

namespace Bookgen.Lib.VFS;

public sealed class Folder : IFolder, IReadOnlyFolder
{
    private readonly string _rootFolder;

    public string FullPath => _rootFolder;

    private static string GetFullPath(string file)
    {
        return System.Environment.OSVersion.Platform == PlatformID.Unix
            ? Path.GetFullPath(file.Replace('\\', '/'))
            : Path.GetFullPath(file.Replace('/', '\\'));
    }

    private void Validate(string file)
    {
        if (!file.StartsWith(_rootFolder))
            throw new InvalidOperationException($"{file} is not in {_rootFolder}");
    }

    public Folder(string rootFolder)
    {
        _rootFolder = GetFullPath(rootFolder);
    }

    public TextReader OpenText(string path)
    {
        string actualPath = GetFullPath(path);
        Validate(actualPath);
        return File.OpenText(actualPath);
    }

    public Stream OpenStream(string path)
    {
        string actualPath = GetFullPath(path);
        Validate(actualPath);
        return File.OpenRead(actualPath);
    }

    public bool Exists(string path)
    {
        string actualPath = GetFullPath(path);
        return File.Exists(actualPath);
    }

    public DateTime GetLastModifiedUtc(string path)
    {
        string actualPath = GetFullPath(path);
        Validate(actualPath);
        return File.GetLastWriteTimeUtc(actualPath);
    }

    public string ReadText(string path)
    {
        string actualPath = GetFullPath(path);
        Validate(actualPath);
        return File.ReadAllText(actualPath);
    }

    public Stream CreateStream(string path)
    {
        string actualPath = GetFullPath(path);
        return File.Create(actualPath);
    }

    public void Delete(string path)
    {
        string actualPath = GetFullPath(path);
        File.Delete(actualPath);
    }

    public async Task WriteTextAsync(string path, string content)
    {
        string actualPath = GetFullPath(path);
        await File.WriteAllTextAsync(actualPath, content);
    }

    public async Task CopyTo(string path, IFolder destination)
    {
        string sourcePath = GetFullPath(path);
        Validate(sourcePath);

        string destinationPath = Path.Combine(destination.FullPath, Path.GetFileName(sourcePath));
        await using FileStream sourceStream = File.OpenRead(sourcePath);
        await using FileStream destinationStream = File.Create(destinationPath);

        await sourceStream.CopyToAsync(destinationStream);

    }

    public void CreateIfNotExist()
    {
        if (Directory.Exists(FullPath))
            return;

        Directory.CreateDirectory(FullPath);
    }

    public IEnumerable<string> GetFiles(bool recursive)
        => Directory.EnumerateFiles(FullPath, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    

    public IEnumerable<string> GetDirectories(bool recursive)
        => Directory.EnumerateDirectories(FullPath, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
}
