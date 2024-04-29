//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shell.GitGui;

internal sealed class DelegateCommand : GuiCommand

{
    private readonly Action _action;

    public DelegateCommand(string displayName, Action action)
    {
        DisplayName = displayName;
        _action = action;
    }

    public override string DisplayName { get; }

    public override int Execute(string workDir, IProgress<string> progress)
    {
        _action.Invoke();
        return 0;
    }
}
