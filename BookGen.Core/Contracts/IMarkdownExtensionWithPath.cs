//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;

namespace BookGen.Core.Contracts
{
    public interface IMarkdownExtensionWithPath: IMarkdownExtension
    {
        FsPath Path { get; set; }
    }
}
