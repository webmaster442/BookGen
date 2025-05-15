//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure;

internal interface IMutexFolderLock
{
    bool CheckAndLock(string folderToCheck);
    void CheckLockFileExistsAndExitWhenNeeded(ILogger log, string folder);
}