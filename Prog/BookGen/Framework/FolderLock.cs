//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Native;

namespace BookGen.Framework;

internal static class FolderLock
{
    public static bool IsFolderLocked(string folderToCheck)
    {
        var processExtensons = NativeFactory.GetPlatformProcessExtensions();

        if (!Directory.Exists(folderToCheck))
            return false;

        Environment.CurrentDirectory = folderToCheck;

#if DEBUG
        var directory = processExtensons.GetWorkingDirectory(Process.GetCurrentProcess());
        if (Environment.CurrentDirectory != directory)
        {
            //Working dir not correctly set
            Debugger.Break();
        }
#endif

        var concurrentBookGens = Process.GetProcesses()
            .Where(p => p.ProcessName == "BookGen"
              && p.Id != Environment.ProcessId);

        foreach (var otherBookGen in concurrentBookGens) 
        {
            var workingFolder = processExtensons.GetWorkingDirectory(otherBookGen);
            if (string.IsNullOrEmpty(workingFolder)
                || workingFolder == folderToCheck)
            {
                return true;
            }
        }

        return false;
    }
}
