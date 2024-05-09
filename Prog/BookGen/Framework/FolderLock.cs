//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Threading; 

namespace BookGen.Framework;

internal static class FolderLock
{
    private static Mutex? _lockMutex;

    public static bool IsFolderLocked(string folderToCheck)
    {
        if (!Directory.Exists(folderToCheck))
            return false;

        Environment.CurrentDirectory = folderToCheck;

        var mutexId = Convert.ToBase64String(Encoding.UTF8.GetBytes(folderToCheck));

        if (Mutex.TryOpenExisting(mutexId, out _))
        {
            return true;
        }

        _lockMutex = new Mutex(true, mutexId);
        return true;
    }

    public static void ReleaseLock()
    {
        if (_lockMutex != null)
        {
            _lockMutex.ReleaseMutex();
            _lockMutex.Dispose();
        }
    }
}
