using BookGen.Framework;

namespace BookGen.Infrastructure;

internal static class Extensions
{
    public static void CheckLockFileExistsAndExitWhenNeeded(this ILog log, string folder)
    {
        log.Info("Checking folder lock status...");
        if (FolderLock.IsFolderLocked(folder))
        {
            log.Critical("An other bookgen process is using this folder. Exiting...");
            Environment.Exit((int)ExitCode.FolderLocked);
        }
    }

}
