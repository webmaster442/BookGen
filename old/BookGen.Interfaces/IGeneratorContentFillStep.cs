//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces
{
    public interface IGeneratorContentFillStep : IGeneratorStep
    {
        IContent? Content { get; set; }
    }
}
