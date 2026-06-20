using Markdig.Renderers;
using Markdig.Syntax;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

public abstract class TerminalObjectRenderer<TObject> : MarkdownObjectRenderer<TerminalRenderer, TObject> where TObject : MarkdownObject
{
}
