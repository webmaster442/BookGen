//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

/// <summary>
/// Base class for async command without settings.
/// </summary>
public abstract class AsyncCommand : ICommand
{
    public abstract Task<int> ExecuteAsync(IReadOnlyList<string> context, CancellationToken token);

    Task<int> ICommand.ExecuteAsync(ArgumentsBase arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        return ExecuteAsync(context, token);
    }

    public virtual SupportedOs SupportedOs
        => SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX;
}
