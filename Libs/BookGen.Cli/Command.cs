﻿//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

/// <summary>
/// Base class for a command without settings.
/// </summary>
public abstract class Command : ICommand
{
    /// <summary>
    /// Entry point of command
    /// </summary>
    /// <param name="context">arguments passed to command</param>
    /// <returns>reurtn value to shell</returns>
    public abstract int Execute(string[] context);

    Task<int> ICommand.Execute(ArgumentsBase arguments, string[] context)
    {
        return Task.FromResult(Execute(context));
    }

    public virtual SupportedOs SupportedOs
        => SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX;
}
