//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

        public static bool TryCheckLockExistance(string folder, out string lockFile)
        {
            lockFile = Path.Combine(folder, lockName);
            return File.Exists(lockFile);
        }
    }
}
