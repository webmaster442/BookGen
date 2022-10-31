//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;

namespace BookGen.Update.Steps;

internal sealed class Finish : IUpdateStepSync
{
    public string StatusMessage => string.Empty;

    public bool Execute(GlobalState state)
    {
        Console.WriteLine($"Successfully updated to {state.Latest.Version}");
        state.Cleanup();
        return true;
    }
}
