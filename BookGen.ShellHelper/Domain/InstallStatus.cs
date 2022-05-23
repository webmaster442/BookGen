//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ShellHelper.Domain
{
    public sealed record class InstallStatus
    {
        public bool IsWindowsTerminalInstalled { get; init; }
        public bool IsVSCodeInstalled { get; init; }
        public bool IsPsCoreInstalled { get; init; }
    }
}
