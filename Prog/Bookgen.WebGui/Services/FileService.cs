//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

using BookGen.WebGui.Domain;

namespace BookGen.WebGui.Services;

internal sealed class FileService : IFileService
{
    private Dictionary<string, string> _directories;
    private Dictionary<string, string> _files;
    private readonly ICurrentSession _currentSession;
    private readonly string _startDirId;

    public FileService(ICurrentSession currentSession)
    {
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
}
