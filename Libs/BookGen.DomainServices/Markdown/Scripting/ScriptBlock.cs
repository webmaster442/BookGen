//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Syntax;

namespace BookGen.DomainServices.Markdown.Scripting;

internal sealed class ScriptBlock : FencedCodeBlock
{
    public ScriptBlock(BlockParser parser) : base(parser)
    {
    }

    public string GetScript()
    {
        return string.Join(Environment.NewLine, Lines);
    }
}
