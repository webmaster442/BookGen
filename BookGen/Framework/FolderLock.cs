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
        private string _lockfile;
        private const string lockName = "bookGen.lock";

        public FolderLock(string folder)
        {
            _lockfile = Path.Combine(folder, lockName);
            if (!File.Exists(_lockfile))
            {
                File.CreateText(_lockfile);
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
            if (File.Exists(Path.Combine(folder, lockName)))
            {
                log.Critical("An other bookgen process is using this folder. Exiting...");
                Environment.Exit((int)ExitCode.FolderLocked);
            }
        }
    }
}
