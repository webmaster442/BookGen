using System.Diagnostics;

namespace BookGen.Vfs;

public sealed class FolderLock : IDisposable
{
    private readonly string _lockPath;
    private readonly IWritableFileSystem _writableFileSystem;

    public FolderLock(IWritableFileSystem writableFileSystem, string lockFileName)
    {
        _lockPath = Path.Combine(writableFileSystem.Scope, lockFileName);
        _writableFileSystem = writableFileSystem;
    }

    public bool Initialize()
    {
        static bool IsRunning(int id)
            => Process.GetProcesses().Any(p => p.Id == id);

        var process = Process.GetCurrentProcess();
        var lockObject = new Lock();
        lock (lockObject)
        {
            if (_writableFileSystem.FileExists(_lockPath))
            {
                var id = _writableFileSystem.ReadAllText(_lockPath).Trim();
                if (int.TryParse(id, out int pid) && IsRunning(pid))
                {
                    return false;
                }
                return true;
            }

            _writableFileSystem.WriteAllText(_lockPath, Environment.ProcessId.ToString());
            return true;
        }
    }

    public void Dispose()
    {
        if (_writableFileSystem.FileExists(_lockPath))
        {
            _writableFileSystem.Delete(_lockPath);
        }
    }
}
