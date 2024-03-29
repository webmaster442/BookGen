﻿//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels.FileBrowser;

internal static class ModelsFactory
{
    public static IEnumerable<FileBrowserItemViewModel> CreateItemModels(string path)
    {
        string[] dirs = GetFilesSafe(path);

        foreach (string file in dirs)
            yield return CreateFileModel(file);
    }

    public static IEnumerable<FileBrowserTreeViewModel> CreateTreeModels(string path)
    {
        yield return new FileBrowserTreeViewModel
        {
            FullPath = path,
            Name = GetFileName(path),
            SubItems = GetSubItems(path),
        };
    }

    private static string[] GetFilesSafe(string path)
    {
        try
        {
            return Directory.GetFiles(path);
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<string>();
        }
    }

    private static string GetFileName(string path)
    {
        var name = Path.GetFileName(path);
        if (string.IsNullOrEmpty(name))
            return path;
        return name;
    }

    private static FileBrowserTreeViewModel[] GetSubItems(string path)
    {
        try
        {
            return Directory.GetDirectories(path).Select(x =>
                new FileBrowserTreeViewModel
                {
                    FullPath = x,
                    Name = GetFileName(x),
                    SubItems = GetSubItems(x)
                }
            ).ToArray();
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<FileBrowserTreeViewModel>();
        }
    }

    private static FileBrowserItemViewModel CreateFileModel(string file)
    {
        var fi = new FileInfo(file);

        return new FileBrowserItemViewModel
        {
            ModificationDate = fi.LastWriteTime,
            Extension = fi.Extension,
            FullPath = file,
            Size = fi.Length,
            Name = fi.Name,
        };
    }
}
