//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Contracts.Configuration
{
    public interface IReadOnlyAsset
    {
        string Source { get; }
        string Target { get; }
    }
}
