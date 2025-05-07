
namespace Bookgen.Lib.VFS;

public interface IFolder : IReadOnlyFolder
{
    Stream CreateStream(string path);
    void Delete(string path);
    Task WriteTextAsync(string path, string content);
    Task CopyTo(string path, IFolder destination);
    void CreateIfNotExist();
}
