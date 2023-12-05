//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using TextResources = BookGen.Shell.Properties.Resources;

namespace BookGen.Shell.Cdg;

internal sealed class DirectoriesProvider
{
    private readonly Dictionary<string, string> _knownFolders;

    public DirectoriesProvider()
    {
        _knownFolders = new Dictionary<string, string>();
        foreach (var folder in Enum.GetValues<Environment.SpecialFolder>().Distinct())
        {
            string path = Environment.GetFolderPath(folder);
            if (!string.IsNullOrEmpty(path))
            {
                _knownFolders.Add($"|-{folder}", path);
            }
        }
    }

    public static bool PathIsRootDirString(string input) => input == nameof(TextResources._MenuSelectorRootDir_30);

    public static bool PathIsCurrentDirString(string input) => input == nameof(TextResources._MenuSelectorCurrentDir_10);

    public static bool PathIsHomeDirString(string input) => input == nameof(TextResources._MenuSelectorHomeDir_35);

    public static bool PathIsKnownDirsString(string input) => input == nameof(TextResources._MenuSelectorKnownDirs_40);

    public bool TryKnownFolder(string selected, out string newFolder)
    {
        if (_knownFolders.TryGetValue(selected, out string? value))
        {
            newFolder = value;
            return true;
        }
        newFolder = string.Empty;
        return false;
    }

    public static bool TryUpOneDir(string dir, string path, out string oneDirUp)
    {
        if (dir == nameof(TextResources._MenuSelectorUpOneDir_20))
        {
            DirectoryInfo di = new(path);
            var parent = di.Parent;
            if (parent != null)
            {
                oneDirUp = parent.FullName;
            }
            else
            {
                oneDirUp = nameof(TextResources._MenuSelectorRootDir_30);
            }
            return true;
        }
        oneDirUp = path;
        return false;
    }

    public IEnumerable<string> GetSubdirs(string workDir, bool showHidden)
    {
        if (PathIsRootDirString(workDir))
        {
            return GetDrives();
        }
        else if (PathIsKnownDirsString(workDir))
        {
            return _knownFolders.Select(x => x.Key).Order();
        }
        return GetDirectories(workDir, showHidden);
    }

    private static IEnumerable<string> GetDrives()
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady)
                yield return drive.Name;
        }
    }

    private static IEnumerable<string> GetDirectories(string workDir, bool showHidden)
    {
        DirectoryInfo directory = new(workDir);

        foreach (var subdir in directory.GetDirectories())
        {
            if (subdir.Attributes.HasFlag(FileAttributes.Hidden)
                && !showHidden)
            {
                continue;
            }
            yield return subdir.FullName;
        }
    }
}
