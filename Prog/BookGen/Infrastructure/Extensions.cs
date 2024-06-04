//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Framework;

namespace BookGen.Infrastructure;

internal static class Extensions
{
    public static void CheckLockFileExistsAndExitWhenNeeded(this IMutexFolderLock folderLock, ILog log, string folder)
    {
        log.Info("Checking folder lock status...");
        if (folderLock.CheckAndLock(folder))
        {
            log.Critical("An other bookgen process is using this folder. Exiting...");
            Environment.Exit(Constants.FolderLocked);
        }
    }

    public static void EnableVerboseLogingIfRequested(this ILog log, BookGenArgumentBase argumentBase)
    {
        if (argumentBase.Verbose)
            log.LogLevel = LogLevel.Detail;
    }
}
