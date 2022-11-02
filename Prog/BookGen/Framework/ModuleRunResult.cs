//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Framework
{
    internal enum ModuleRunResult
    {
        Succes = 0,
        ArgumentsError = 1,
        AsyncModuleCalledInSyncMode = int.MaxValue - 1,
        GeneralError = int.MaxValue,
    }
}
