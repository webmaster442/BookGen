//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shell.GitGui;

internal abstract class GuiCommand
{
    public abstract string DisplayName { get; }
    public abstract int Execute(string workDir, IProgress<string> progress);
}
