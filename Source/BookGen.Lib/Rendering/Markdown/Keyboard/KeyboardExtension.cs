//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace BookGen.Lib.Rendering.Markdown.Keyboard;

internal sealed class KeyboardExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        OrderedList<InlineParser> parsers = pipeline.InlineParsers;
        if (!parsers.Contains<KeyboardInlineParser>())
        {
            parsers.Add(new KeyboardInlineParser());
        }
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            ObjectRendererCollection renderers = htmlRenderer.ObjectRenderers;
            if (!renderers.Contains<KeyboardHtmlRenderer>())
            {
                renderers.Add(new KeyboardHtmlRenderer());
            }
        }
    }
}
