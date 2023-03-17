using BookGen.Framework;
using System.IO;

namespace BookGen.Infrastructure
{
    internal static class Extensions
    {
        public static void CheckLockFileExistsAndExitWhenNeeded(this ILog log, string folder)
        {
            if (FolderLock.TryCheckLockExistance(folder, out string lockfile))
            {
#if DEBUG
                System.Diagnostics.Process[]? running = System.Diagnostics.Process.GetProcessesByName(nameof(BookGen));
                if (running.Length == 1) //only this current instance is running
                {
                    log.Warning("Lockfile was found, but no other bookgen is running. Removing lock...");
                    File.Delete(lockfile);
                    return;
                }
#endif
                log.Critical("An other bookgen process is using this folder. Exiting...");
                Environment.Exit((int)ExitCode.FolderLocked);
            }
        }

    }
}
