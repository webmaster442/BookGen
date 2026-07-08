//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Syntax;

namespace BookGen.Lib.Rendering.Markdown.TableOfContents;

internal sealed class TocBlock : HeadingBlock
{
    public TocBlock(BlockParser parser) : base(parser)
    {
        ProcessInlines = true;
    }
}
