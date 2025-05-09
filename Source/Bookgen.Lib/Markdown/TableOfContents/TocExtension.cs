//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

using BookGen.DomainServices.Markdown.TableOfContents;

using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.TableOfContents;

/// <summary>
/// Table of content extension
/// </summary>
internal sealed class TocExtension : IMarkdownExtension
{
    /// <summary>
    /// Options of TocExtension
    /// </summary>
    public TocState State { get; }

    /// <summary>
    /// Construct a new instance.
    /// </summary>
    /// <param name="state">Options of TocExtension</param>
    public TocExtension(TocState state)
    {
        State = state ?? new TocState();
    }

    //register parsers
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        var autoIdExtension = pipeline.Extensions.Find<CustomAutoIdExtension>();
        if (autoIdExtension == null)
            throw new InvalidOperationException("CustomAutoIdExtension is null");

        autoIdExtension.OnHeadingParsed -= AutoIdExtension_OnHeadingParsed;
        autoIdExtension.OnHeadingParsed += AutoIdExtension_OnHeadingParsed;
        pipeline.BlockParsers.AddIfNotAlready(new TocBlockParser(State));
    }

    private void AutoIdExtension_OnHeadingParsed(HeadingInfo heading)
        => State.AddHeading(heading);

    //register randerers, this method will execute after all parsers completed.
    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            //because TocBlock:HeadingBlock ,so renderer must before HeadingRenderer
            if (!htmlRenderer.ObjectRenderers.Contains<HtmlTocRenderer>())
                htmlRenderer.ObjectRenderers.InsertBefore<HeadingRenderer>(new HtmlTocRenderer(State));
        }

        renderer.ObjectWriteAfter += Renderer_ObjectWriteAfter;
    }

    private void Renderer_ObjectWriteAfter(IMarkdownRenderer arg1, MarkdownObject arg2)
    {
        //move HtmlTocRenderer -> Write -> 'Options.Headings.Clear()' to here
        //now it can be more than one [toc] in markdown document , and all of these can be rendered
        //though it's useless
        if (arg2 is MarkdownDocument)// when the document all writed
            State.Headings.Clear();
    }
}
