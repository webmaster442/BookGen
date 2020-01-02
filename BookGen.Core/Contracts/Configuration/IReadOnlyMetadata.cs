//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Contracts.Configuration
{
    public interface IReadOnlyMetadata
    {
        string Author { get; }
        string CoverImage { get; }
        string Title { get; }
    }
}