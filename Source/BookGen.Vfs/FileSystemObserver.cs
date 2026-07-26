//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace BookGen.Vfs;

public sealed class FileSystemObserver : IFileSystemObserver
{
    private readonly FileSystemWatcher _watcher;
    private readonly ILogger _logger;

    public FileSystemObserver(ILogger logger, string folder, string filter)
    {
        _logger = logger;
        _watcher = new FileSystemWatcher
        {
            Filter = filter,
            Path = folder,
            NotifyFilter = NotifyFilters.Attributes
                         | NotifyFilters.DirectoryName
                         | NotifyFilters.FileName
                         | NotifyFilters.LastWrite
                         | NotifyFilters.Size,
            IncludeSubdirectories = true,
        };
        _watcher.Changed += OnChanged;
        _watcher.Created += OnChanged;
        _watcher.Deleted += OnChanged;
        _watcher.Renamed += OnRenamed;
        _watcher.Error += OnError;
        _watcher.EnableRaisingEvents = true;
    }

    public void Dispose()
    {
        _watcher.Changed -= OnChanged;
        _watcher.Created -= OnChanged;
        _watcher.Deleted -= OnChanged;
        _watcher.Renamed -= OnRenamed;
        _watcher.Error -= OnError;
        _watcher.Dispose();
    }

    public event EventHandler<FileSystemChangeEventArgs>? FileChanged;

    private void OnError(object sender, ErrorEventArgs e)
    {
        _logger.LogError(e.GetException(), "File system watcher error");
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        FileChanged?.Invoke(this, new FileSystemChangeEventArgs
        {
            FileName = e.OldFullPath,
            NewFileName = e.FullPath,
            ChangeType = FileSystemChangeEventArgs.Change.Renamed,
        });
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        FileChanged?.Invoke(this, new FileSystemChangeEventArgs
        {
            FileName = e.FullPath,
            ChangeType = Map(e.ChangeType),
        });
    }

    private static FileSystemChangeEventArgs.Change Map(WatcherChangeTypes changeType)
    {
        return changeType switch
        {
            WatcherChangeTypes.Created => FileSystemChangeEventArgs.Change.Created,
            WatcherChangeTypes.Changed => FileSystemChangeEventArgs.Change.Changed,
            WatcherChangeTypes.Deleted => FileSystemChangeEventArgs.Change.Deleted,
            _ => throw new UnreachableException()
        };
    }
}
