//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;

namespace BookGen.Core.Contracts
{
    public interface ITemplateShortCode
    {
        string Tag { get; }
        string Generate(IArguments arguments);
        bool CanCacheResult { get; }
    }
}
