//-----------------------------------------------------------------------------
// (c) 2021-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure;

internal sealed class MutexFolderLock : IDisposable, IMutexFolderLock
{
    private Mutex? _lockMutex;

    public MutexFolderLock()
    {
        Console.CancelKeyPress += OnConsoleAbort;
    }

    public bool CheckAndLock(string folderToCheck)
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
        return false;
    }

    public void CheckLockFileExistsAndExitWhenNeeded(ILogger log, string folder)
    {
        log.LogInformation("Checking folder lock status...");
        if (CheckAndLock(folder))
        {
            log.LogCritical("An other bookgen process is using this folder. Exiting...");
            Environment.Exit(ExitCodes.FolderLocked);
        }
    }

    public void Dispose()
    {
        Console.CancelKeyPress -= OnConsoleAbort;
        _lockMutex?.Dispose();
    }

    private void OnConsoleAbort(object? sender, ConsoleCancelEventArgs e)
    {
        Dispose();
    }
}
