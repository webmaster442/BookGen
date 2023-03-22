//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli
{
    public interface ICommand
    {
        Task<int> Execute(ArgumentsBase arguments, string[] context);
        SupportedOs SupportedOs { get; }
    }
}
