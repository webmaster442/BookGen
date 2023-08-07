//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Native.Implementations;

internal class DumyProcessExtensions : IProcessExtensions
{
    public string GetWorkingDirectory(Process process)
    {
        return string.Empty;
    }
}