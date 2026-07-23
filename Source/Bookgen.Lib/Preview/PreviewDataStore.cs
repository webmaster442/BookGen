using BookGen.Vfs;

using Microsoft.Extensions.Caching.Memory;

namespace BookGen.Lib.Preview;

internal class PreviewDataStore
{
    private readonly IMemoryCache _memoryCache;
    private readonly List<string> _availableFiles;

    public PreviewDataStore(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _availableFiles = new List<string>();
    }

    public IReadOnlyList<string> Files
        => _availableFiles;

    private string GetCacheKey(string path)
        => $"preview_{path}";

    public void UpdateAvailableFiles(IEnumerable<string> enumerable)
    {
        _availableFiles.Clear();
        _availableFiles.AddRange(enumerable);
    }

    public void UpdateWith(FileSystemChangeEventArgs e)
    {
        //Invalidate previous rendering
        if (!string.IsNullOrEmpty(e.FileName))
        {
            _memoryCache.Remove(GetCacheKey(e.FileName));
        }
        if (!string.IsNullOrEmpty(e.NewFileName))
        {
            _memoryCache.Remove(GetCacheKey(e.NewFileName));
        }

        switch (e.ChangeType)
        {
            case FileSystemChangeEventArgs.Change.Created:
                _availableFiles.Add(e.FileName);
                break;
            case FileSystemChangeEventArgs.Change.Changed:
                // No action needed for changed files
                break;
            case FileSystemChangeEventArgs.Change.Deleted:
                _availableFiles.Remove(e.FileName);
                break;
            case FileSystemChangeEventArgs.Change.Renamed:
                if (!string.IsNullOrEmpty(e.NewFileName))
                {
                    int idx = _availableFiles.IndexOf(e.FileName);
                    if (idx != -1)
                    {
                        _availableFiles[idx] = e.NewFileName;
                    }
                    else
                    {
                        _availableFiles.Add(e.NewFileName);
                    }
                }
                break;
        }
    }
}
