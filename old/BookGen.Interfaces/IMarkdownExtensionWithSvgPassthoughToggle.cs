//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces
{
    public interface IMarkdownExtensionWithSvgPassthoughToggle
    {
        bool SvgPasstrough { get; set; }
    }
}
