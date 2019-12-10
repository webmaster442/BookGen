//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;

namespace BookGen.Contracts
{
    internal interface IGeneratorContentFillStep : IGeneratorStep
    {
        IContent? Content { get; set; }
    }
}
