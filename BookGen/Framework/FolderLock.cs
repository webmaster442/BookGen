//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using System;
using System.IO;

namespace BookGen.Framework
{
    internal sealed class FolderLock : IDisposable
    {
        private readonly string _lockfile;
        private const string lockName = "bookGen.lock";

        public FolderLock(string folder)
        {
            _lockfile = Path.Combine(folder, lockName);
            if (!File.Exists(_lockfile))
            {
                using var f = File.CreateText(_lockfile);
            }
        }

        public void Dispose()
        {
            if (File.Exists(_lockfile))
            {
                File.Delete(_lockfile);
            }
        }

        public static void ExitIfFolderIsLocked(string folder, ILog log)
        {
            var lockfile = Path.Combine(folder, lockName);
            if (File.Exists(lockfile))
            {
#if DEBUG
                var running = System.Diagnostics.Process.GetProcessesByName(nameof(BookGen));
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
