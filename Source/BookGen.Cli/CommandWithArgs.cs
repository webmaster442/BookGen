//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

/// <summary>
/// Base class for a command.
/// </summary>
/// <typeparam name="TArguments">Argument type</typeparam>
public abstract class Command<TArguments> : ICommand
    where TArguments : ArgumentsBase
{
    public abstract int Execute(TArguments arguments, string[] context);

    Task<int> ICommand.Execute(ArgumentsBase arguments, string[] context)
    {
        return Task.FromResult(Execute((TArguments)arguments, context));
    }

    public virtual SupportedOs SupportedOs
        => SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX;
}
