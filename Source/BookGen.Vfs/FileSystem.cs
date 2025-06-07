using System.Diagnostics;

namespace BookGen.Vfs;

[DebuggerDisplay("{Scope}")]
public sealed class FileSystem : ReadOnlyFileSystem, IWritableFileSystem, IReadOnlyFileSystem
{
    private static void CreatePathIfNeeded(string file)
    {
        var dir = Path.GetDirectoryName(file);

        if (string.IsNullOrEmpty(dir))
            throw new DirectoryNotFoundException(nameof(dir));

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    public FileSystem(string scope = "") : base(scope)
    {
    }

    public async Task CopyToAsync(string path, string destination)
    {
        static async Task CopySingleFile(string source, string destination)
        {
            await using var sourceStream = File.OpenRead(source);
            await using var destinationStream = File.Create(destination);
            await sourceStream.CopyToAsync(destinationStream);
        }

        var actualPath = GetAndValidateFullNameInScope(path);
        if (Directory.Exists(actualPath))
        {
            var files = Directory.EnumerateFiles(actualPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                CreatePathIfNeeded(file);
                await CopySingleFile(file, actualPath);
            }
        }
        else
        {
            CreatePathIfNeeded(destination);
            await CopySingleFile(actualPath, destination);
        }
    }

    public void CreateDirectoryIfNotExist(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        if (!string.IsNullOrEmpty(actualPath) && !Directory.Exists(actualPath))
        {
            Directory.CreateDirectory(actualPath);
        }
    }

    public TextWriter CreateTextWriter(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        CreatePathIfNeeded(actualPath);
        return File.CreateText(actualPath);
    }

    public Stream CreateWriteStream(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        CreatePathIfNeeded(actualPath);
        return File.Create(actualPath);
    }

    public void Delete(string path)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        if (Directory.Exists(actualPath))
            Directory.Delete(actualPath, true);
        else
            File.Delete(actualPath);
    }

    public void MoveFile(string path, string destination)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        var destinationPath = GetAndValidateFullNameInScope(destination);
        File.Move(actualPath, destinationPath);
    }

    public void WriteAllText(string path, string content)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        CreatePathIfNeeded(actualPath);
        File.WriteAllText(actualPath, content);
    }

    public async Task WriteAllTextAsync(string path, string content)
    {
        var actualPath = GetAndValidateFullNameInScope(path);
        CreatePathIfNeeded(actualPath);
        await File.WriteAllTextAsync(actualPath, content);
    }
}
