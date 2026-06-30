using Markdig.Renderers;

using Webmaster442.WindowsTerminal;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

public sealed class TerminalRenderer : TextRendererBase<TerminalRenderer>
{

    public TerminalRenderer(TextWriter writer, RenderOptions renderOptions) : base(writer)
    {
        RenderOptions = renderOptions;
        Builder = new TerminalFormattedStringBuilder();

        ObjectRenderers.Add(new CodeBlockRenderer());
        ObjectRenderers.Add(new ListRenderer());
        ObjectRenderers.Add(new HeadingRenderer());
        ObjectRenderers.Add(new ParagraphRenderer());
        ObjectRenderers.Add(new QuoteBlockRenderer());
        ObjectRenderers.Add(new ThematicBreakRenderer());

        ObjectRenderers.Add(new AutolinkInlineRenderer());
        ObjectRenderers.Add(new CodeInlineRenderer());
        ObjectRenderers.Add(new DelimiterInlineRenderer());
        ObjectRenderers.Add(new EmphasisInlineRenderer());
        ObjectRenderers.Add(new LineBreakInlineRenderer());
        ObjectRenderers.Add(new LinkInlineRenderer());
        ObjectRenderers.Add(new LiteralInlineRenderer());
    }

    public RenderOptions RenderOptions { get; }

    internal TerminalFormattedStringBuilder Builder { get; }

    private const string ResetCode = "\u001b[0m";

    public TerminalRenderer WriteReset()
    {
        Write(ResetCode);
        return this;
    }
}
