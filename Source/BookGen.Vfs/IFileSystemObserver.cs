namespace BookGen.Vfs;

public interface IFileSystemObserver : IDisposable
{
    event EventHandler<FileSystemChangeEventArgs>? FileChanged;
}
