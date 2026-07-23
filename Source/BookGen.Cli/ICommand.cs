//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public interface ICommand
{
    Task<int> ExecuteAsync(ArgumentsBase arguments, IReadOnlyList<string> context, CancellationToken token);
    SupportedOs SupportedOs { get; }
}
