//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;

namespace BookGen.Interfaces
{
    public interface IMarkdownExtensionWithRuntimeConfig : IMarkdownExtension
    {
        IReadonlyRuntimeSettings? RuntimeConfig { get; set; }
    }
}
