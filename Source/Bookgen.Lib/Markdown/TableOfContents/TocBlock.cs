//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.TableOfContents;

internal sealed class TocBlock : HeadingBlock
{
    public TocBlock(BlockParser parser) : base(parser)
    {
        ProcessInlines = true;
    }
}
