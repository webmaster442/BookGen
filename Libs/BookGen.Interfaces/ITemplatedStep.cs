//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces
{
    internal interface ITemplatedStep : IGeneratorContentFillStep
    {
        ITemplateProcessor? Template { get; set; }
    }
}
