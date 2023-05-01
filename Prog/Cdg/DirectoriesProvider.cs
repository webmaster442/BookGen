using Cdg.Properties;

namespace Cdg;

internal class DirectoriesProvider
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

    public static bool PathIsRootDirString(string input) => input == nameof(Resources._MenuSelectorRootDir_30);

    public static bool PathIsCurrentDirString(string input) => input == nameof(Resources._MenuSelectorCurrentDir_10);

    public static bool PathIsHomeDirString(string input) => input == nameof(Resources._MenuSelectorHomeDir_35);

    public static bool PathIsKnownDirsString(string input) => input == nameof(Resources._MenuSelectorKnownDirs_40);

    public bool TryKnownFolder(string selected, out string newFolder)
    {
        if (_knownFolders.ContainsKey(selected))
        {
            newFolder = _knownFolders[selected];
            return true;
        }
        newFolder = string.Empty;
        return false;
    }

    public bool TryUpOneDir(string dir, string path, out string oneDirUp)
    {
        if (dir == nameof(Resources._MenuSelectorUpOneDir_20))
        {
            DirectoryInfo di = new(path);
            var parent = di.Parent;
            if (parent != null)
            {
                oneDirUp = parent.FullName;
            }
            else
            {
                oneDirUp = nameof(Resources._MenuSelectorRootDir_30);
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
            return _knownFolders.Select(x => x.Key.ToString()).Order();
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
        yield break;
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
