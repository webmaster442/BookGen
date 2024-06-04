//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Framework;

internal interface IMutexFolderLock
{
    bool CheckAndLock(string folderToCheck);
}