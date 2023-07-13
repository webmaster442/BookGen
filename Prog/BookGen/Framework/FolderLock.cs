//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Framework;

internal static class FolderLock
{
    public static bool IsFolderLocked(string folder)
    {
        if (!Directory.Exists(folder))
            return false;

        var concurrentProcesses = Process.GetProcesses()
            .Where(p => p.StartInfo.WorkingDirectory == folder)
            .Where(p => p.ProcessName == "BookGen")
            .Where(p => p.Id != Environment.ProcessId)
            .Any();

        return concurrentProcesses;
    }
}
