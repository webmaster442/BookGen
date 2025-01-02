//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Framework;

namespace BookGen.Infrastructure;

internal static class Extensions
{
    public static void CheckLockFileExistsAndExitWhenNeeded(this IMutexFolderLock folderLock, ILogger log, string folder)
    {
        log.LogInformation("Checking folder lock status...");
        if (folderLock.CheckAndLock(folder))
        {
            log.LogCritical("An other bookgen process is using this folder. Exiting...");
            Environment.Exit(Constants.FolderLocked);
        }
    }
}
