//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

using BookGen.Interfaces;
using BookGen.WebGui.Domain;
using BookGen.Web;

namespace BookGen.WebGui.Services;

internal sealed class FileService : IFileService
{
    private Dictionary<string, string> _directories;
    private Dictionary<string, string> _files;
    private readonly HashSet<string> _previewables;
    private readonly ICurrentSession _currentSession;
    private readonly string _startDirId;

    public FileService(ICurrentSession currentSession)
    {
        _previewables = [".md", ".txt", ".ps", ".cmd", ".json", ".yaml"];
        _currentSession = currentSession;
        _startDirId = GetId(_currentSession.StartDirectory.ToString());
        RefreshDirectories();
    }

    [MemberNotNull(nameof(_directories))]
    [MemberNotNull(nameof(_files))]
    private void RefreshDirectories()
    {
        var options = new EnumerationOptions
        {
            AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
            MaxRecursionDepth = byte.MaxValue,
            ReturnSpecialDirectories = false,
            MatchType = MatchType.Win32,
        };

        _directories = Directory
            .GetDirectories(_currentSession.StartDirectory.ToString(), "*.*", options)
            .ToDictionary(x => GetId(x), x => x);

        _directories.Add(_startDirId, _currentSession.StartDirectory.ToString());

        _files = Directory
            .GetFiles(_currentSession.StartDirectory.ToString(), "*.*", options)
            .ToDictionary(x => GetId(x), x => x);
    }

    public bool IsPreviewSupported(string id)
    {
        string file = _files[id];

        var extension = Path.GetExtension(file).ToLower();

        return _previewables.Contains(extension);
    }

    public Stream GetContent(string id)
    {
        string file = _files[id];
        return File.OpenRead(file);
    }

    public IList<BrowserItem> GetFiles(string id)
    {
        if (string.IsNullOrEmpty(id))
            id = GetId(_currentSession.StartDirectory.ToString());

        string fullPath = _directories[id];

        List<BrowserItem> list = new List<BrowserItem>();
        DirectoryInfo parent = new DirectoryInfo(fullPath);

        if (id != _startDirId)
        {
            var previous = Path.GetFullPath(Path.Combine(fullPath, ".."));

            list.Add(new BrowserItem
            {
                Extension = "DIR",
                Id = GetId(previous),
                LastModified = DateTime.Now,
                Name = "⬅️ Back",
                Size = 0,
                IsDirectory = true
            });
        }

        foreach (var directory in parent.GetDirectories())
        {
            list.Add(new BrowserItem
            {
                Extension = "DIR",
                Id = GetId(directory.FullName),
                LastModified = directory.LastWriteTime,
                Name = directory.Name,
                Size = 0,
                IsDirectory = true
            });
        }
        foreach (var file in parent.GetFiles())
        {
            list.Add(new BrowserItem
            {
                Extension = file.Extension.ToUpper(),
                Id = GetId(file.FullName),
                LastModified = file.LastWriteTime,
                Name = file.Name,
                Size = file.Length,
                IsDirectory = false
            });
        }
        return list;
    }

    private static string GetId(string x)
        => Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(x)));

    public bool IsMarkdown(string id)
    {
        string file = _files[id];
        return Path.GetExtension(file).Equals(".md", StringComparison.CurrentCultureIgnoreCase);
    }

    public string GetTextContent(string id)
    {
        string file = _files[id];
        return File.ReadAllText(file);
    }

    public FsPath GetDirectoryOf(string id)
    {
        string file = _files[id];
        var path = Path.GetDirectoryName(file);

        if (string.IsNullOrEmpty(path))
            return _currentSession.StartDirectory;

        return new FsPath(path);
    }

    public string GetFileNameOf(string id)
    {
        string file = _files[id];
        return Path.GetFileName(file);
    }

    public string GetMimeTypeOf(string id)
    {
        string file = _files[id];
        return MimeTypes.GetMimeTypeForFile(file);
    }
}
