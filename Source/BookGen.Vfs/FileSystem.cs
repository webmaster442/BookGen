namespace BookGen.Vfs;

public sealed class FileSystem : IReadOnlyFileSystem, IWritableFileSystem
{
    public FileSystem(string scope = "")
    {
        Scope = scope;
    }

    private string GetAndValidateFullNameInScope(string path)
    {
        string returnValue = string.IsNullOrEmpty(Scope)
            ? Path.GetFullPath(path)
            : Path.GetFullPath(path, Scope);

        if (!returnValue.StartsWith(Scope))
            throw new InvalidOperationException($"{returnValue} points out of defined scope: {Scope}");

        return returnValue;
    }

    public TextReader OpenTextReader(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.OpenText(actualPath);
    }

    public Stream OpenReadStream(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.OpenRead(actualPath);
    }

    public DateTime GetLastModifiedUtc(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return Directory.Exists(actualPath)
            ? Directory.GetLastWriteTimeUtc(actualPath)
            : File.GetLastWriteTimeUtc(actualPath);
    }

    public string ReadAllText(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.ReadAllText(actualPath);
    }

    public async Task<string> ReadAllTextAsync(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return await File.ReadAllTextAsync(actualPath);
    }

    public bool FileExists(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.Exists(actualPath);
    }

    public bool DirectoryExists(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return Directory.Exists(actualPath);
    }

    public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return Directory.EnumerateFiles(actualPath, filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    }

    public IEnumerable<string> GetDirectories(string path, bool recursive)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return Directory.EnumerateFiles(actualPath, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
    }

    public TextWriter CreateTextWriter(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.CreateText(actualPath);
    }

    public Stream CreateWriteStream(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        return File.Create(path);
    }

    public void WriteAllText(string path, string content)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        File.WriteAllText(path, content);
    }

    public async Task WriteAllTextAsync(string path, string content)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        await File.WriteAllTextAsync(path, content);
    }

    public void Delete(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        if (Directory.Exists(actualPath))
            Directory.Delete(actualPath, true);
        else
            File.Delete(actualPath);
    }

    public async Task CopyToAsync(string path, string destination)
    {
        static async Task CopySingleFile(string source, string destination)
        {
            await using var sourceStream = File.OpenRead(source);
            await using var destinationStream = File.Create(destination);
            await sourceStream.CopyToAsync(destinationStream);
        }

        static void CreateDir(string file)
        {
            var dir = Path.GetDirectoryName(file);

            if (string.IsNullOrEmpty(dir))
                throw new DirectoryNotFoundException(nameof(dir));

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        var actualPath = GetAndValidateFullNameInScope(path);
        if (Directory.Exists(actualPath))
        {
            var files = Directory.EnumerateFiles(actualPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                CreateDir(file);
                await CopySingleFile(file, actualPath);
            }
        }
        else
        {
            CreateDir(destination);
            await CopySingleFile(actualPath, destination);
        }
    }

    public void CreateDirectoryIfNotExist(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        var directory = Path.GetDirectoryName(actualPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public string Scope
    {
        get => field;
        set
        {
            field = string.IsNullOrEmpty(value)
                ? value
                : Path.GetFullPath(value);
        }
    }
}