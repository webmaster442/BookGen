//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public interface ICommand
{
    Task<int> ExecuteAsync(ArgumentsBase arguments, IReadOnlyList<string> context);
    SupportedOs SupportedOs { get; }
}
