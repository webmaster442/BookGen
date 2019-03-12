//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;

namespace BookGen.Contracts
{
    internal interface ITemplatedStep : IGeneratorStep, IGeneratorContentFillStep
    {
        Template Template { get; set; }
    }
}
