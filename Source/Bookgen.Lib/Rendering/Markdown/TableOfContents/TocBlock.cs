//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Syntax;

namespace Bookgen.Lib.Rendering.Markdown.TableOfContents;

internal sealed class TocBlock : HeadingBlock
{
    public TocBlock(BlockParser parser) : base(parser)
    {
        ProcessInlines = true;
    }
}
