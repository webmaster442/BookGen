//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Renderers;
using Markdig.Syntax;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

public abstract class TerminalObjectRenderer<TObject> : MarkdownObjectRenderer<TerminalRenderer, TObject> where TObject : MarkdownObject
{
}
