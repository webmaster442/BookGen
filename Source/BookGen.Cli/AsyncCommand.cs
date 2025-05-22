//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

/// <summary>
/// Base class for async command without settings.
/// </summary>
public abstract class AsyncCommand : ICommand
{
    public abstract Task<int> ExecuteAsync(string[] context);

    Task<int> ICommand.ExecuteAsync(ArgumentsBase arguments, string[] context)
    {
        return ExecuteAsync(context);
    }

    public virtual SupportedOs SupportedOs
        => SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX;
}
