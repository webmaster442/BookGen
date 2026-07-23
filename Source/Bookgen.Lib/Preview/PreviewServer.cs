using System;
using System.Collections.Generic;
using System.Text;

using BookGen.Vfs;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen.Lib.Preview;

internal class PreviewServer : IDisposable
{
    private readonly ILogger _logger;
    private readonly PreviewDataStore _dataStore;
    private IFileSystemObserver? _fileSystemObserver;

    public PreviewServer(ILogger logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _dataStore = new PreviewDataStore(memoryCache);
    }

    public void Dispose()
    {
        _fileSystemObserver?.FileChanged -= OnFileChange;
        _fileSystemObserver?.Dispose();
    }

    public async Task Start(IReadOnlyFileSystem fileSystem)
    {
        _dataStore.UpdateAvailableFiles(fileSystem.GetFiles(fileSystem.Scope, "*.md", true));
        _fileSystemObserver = fileSystem.CreateObserver(_logger, "*.md");
        _fileSystemObserver.FileChanged += OnFileChange;
    }

    private void OnFileChange(object? sender, FileSystemChangeEventArgs e)
    {
        _dataStore.UpdateWith(e);
    }
}
