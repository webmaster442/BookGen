//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Contracts
{
    internal interface IGeneratorContentFillStep : IGeneratorStep
    {
        IContent? Content { get; set; }
    }
}
