//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace BookGen.Framework.Scripts;

internal static class ProcessInterop
{
    public static string? ResolveProgramFullPath(string programName, string additional = "")
    {
        string? pathVar = Environment.GetEnvironmentVariable("path");

        if (pathVar == null)
            return null;

        var searchFolders = new List<string>(20);

        if (AppDomain.CurrentDomain.BaseDirectory != null)
            searchFolders.Add(AppDomain.CurrentDomain.BaseDirectory);

        searchFolders.AddRange(pathVar.Split(';'));

        if (!string.IsNullOrEmpty(additional))
            searchFolders.Add(additional);

        foreach (string folder in searchFolders)
        {
            string programFile = Path.Combine(folder, programName);

            if (File.Exists(programFile))
            {
                return programFile;
            }
        }

        return null;
    }

    internal static string AppendExecutableExtension(string file)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return file;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return $"{file}.exe";
        }
        else
        {
            throw new InvalidOperationException("Unknown host operating system");
        }
    }
}
