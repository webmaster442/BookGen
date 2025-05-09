//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Bookgen.Lib.Markdown.TableOfContents;

/// <summary>
/// Custom auto indentifier, add some custom options by <see cref="AutoIdentifierExtension"/>.<br/>
/// </summary>
internal sealed class CustomAutoIdExtension : IMarkdownExtension
{
    private const string AutoIdentifierKey = "AutoIdentifier";
    private readonly CustomAutoIdOptions options;
    private readonly StripRendererCache rendererCache = new StripRendererCache();

    internal event Action<HeadingInfo>? OnHeadingParsed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomAutoIdExtension"/> class.<br/>
    ///  seealso: <seealso cref="AutoIdentifierExtension"/>
    /// </summary>
    /// <param name="options">The options.</param>
    public CustomAutoIdExtension(CustomAutoIdOptions options)
    {
        this.options = options;
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        var headingBlockParser = pipeline.BlockParsers.Find<HeadingBlockParser>();
        if (headingBlockParser is not null)
        {
            // Install a hook on the HeadingBlockParser when a HeadingBlock is actually processed
            headingBlockParser.Closed -= HeadingBlockParser_Closed;
            headingBlockParser.Closed += HeadingBlockParser_Closed;
        }
        var paragraphBlockParser = pipeline.BlockParsers.FindExact<ParagraphBlockParser>();
        if (paragraphBlockParser is not null)
        {
            // Install a hook on the ParagraphBlockParser when a HeadingBlock is actually processed as a Setex heading
            paragraphBlockParser.Closed -= HeadingBlockParser_Closed;
            paragraphBlockParser.Closed += HeadingBlockParser_Closed;
        }
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
    }

    /// <summary>
    /// Process on a new <see cref="HeadingBlock"/>
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="block">The heading block.</param>
    private void HeadingBlockParser_Closed(BlockProcessor processor, Block block)
    {
        // We may have a ParagraphBlock here as we have a hook on the ParagraphBlockParser
        if (block is TocBlock || !(block is HeadingBlock headingBlock))
        {
            return;
        }

        // If the AutoLink options is set, we register a LinkReferenceDefinition at the document level
        if (options.HeadingIdGenerator == null && (options.Options & AutoIdentifierOptions.AutoLink) != 0)
        {
            var headingLine = headingBlock.Lines.Lines[0];

            var text = headingLine.ToString();

            var linkRef = new HeadingLinkReferenceDefinition(headingBlock)
            {
                CreateLinkInline = CreateLinkInlineForHeading
            };

            var doc = processor.Document;
            var dictionary = doc.GetData(this) as Dictionary<string, HeadingLinkReferenceDefinition>;
            if (dictionary is null)
            {
                dictionary = new Dictionary<string, HeadingLinkReferenceDefinition>();
                doc.SetData(this, dictionary);
                doc.ProcessInlinesBegin += DocumentOnProcessInlinesBegin;
            }
            dictionary[text] = linkRef;
        }

        // Then we register after inline have been processed to actually generate the proper #id
        headingBlock.ProcessInlinesEnd += HeadingBlock_ProcessInlinesEnd;
    }

    private void DocumentOnProcessInlinesBegin(InlineProcessor processor, Inline? inline)
    {
        var doc = processor.Document;
        doc.ProcessInlinesBegin -= DocumentOnProcessInlinesBegin;
        var dictionary = (Dictionary<string, HeadingLinkReferenceDefinition>)doc.GetData(this)!;
        foreach (var keyPair in dictionary)
        {
            // Here we make sure that auto-identifiers will not override an existing link definition
            // defined in the document
            // If it is the case, we skip the auto identifier for the Heading
            if (!doc.TryGetLinkReferenceDefinition(keyPair.Key, out var linkDef))
            {
                doc.SetLinkReferenceDefinition(keyPair.Key, keyPair.Value, true);
            }
        }
        // Once we are done, we don't need to keep the intermediate dictionary around
        doc.RemoveData(this);
    }

    /// <summary>
    /// Callback when there is a reference to found to a heading.
    /// Note that reference are only working if they are declared after.
    /// </summary>
    private Inline CreateLinkInlineForHeading(InlineProcessor inlineState, LinkReferenceDefinition linkRef, Inline? child)
    {
        var headingRef = (HeadingLinkReferenceDefinition)linkRef;
        return new LinkInline()
        {
            // Use GetDynamicUrl to allow late binding of the Url (as a link may occur before the heading is declared and
            // the inlines of the heading are actually processed by HeadingBlock_ProcessInlinesEnd)
            GetDynamicUrl = () => HtmlHelper.Unescape("#" + headingRef.Heading.GetAttributes().Id),
            Title = HtmlHelper.Unescape(linkRef.Title),
        };
    }

    /// <summary>
    /// Process the inlines of the heading to create a unique identifier
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="inline">The inline.</param>
    private void HeadingBlock_ProcessInlinesEnd(InlineProcessor processor, Inline? inline)
    {
        var identifiers = processor.Document.GetData(AutoIdentifierKey) as HashSet<string>;
        if (identifiers is null)
        {
            identifiers = new HashSet<string>();
            processor.Document.SetData(AutoIdentifierKey, identifiers);
        }

        var headingBlock = (HeadingBlock)processor.Block!;
        if (headingBlock.Inline is null)
        {
            return;
        }

        // Use internally a HtmlRenderer to strip links from a heading
        var stripRenderer = rendererCache.Get();
        stripRenderer.Render(headingBlock.Inline);
        var headingText = stripRenderer.Writer.ToString()!;
        rendererCache.Release(stripRenderer);

        // If id is already set, don't try to modify it
        var attributes = processor.Block!.GetAttributes();
        try
        {
            if (options.HeadingIdGenerator is not null)
            {
                attributes.Id = options.HeadingIdGenerator(
                    headingBlock.Level, headingText, attributes.Id);
                return;
            }
            if (attributes.Id is not null)
                return;
            var temp = headingText;
            // Urilize the link
            temp = (options.Options & AutoIdentifierOptions.GitHub) != 0
                ? LinkHelper.UrilizeAsGfm(temp)
                : LinkHelper.Urilize(temp, (options.Options & AutoIdentifierOptions.AllowOnlyAscii) != 0);

            // If the heading is empty, use the word "section" instead
            var baseHeadingId = string.IsNullOrEmpty(temp) ? "section" : temp;

            // Add a trailing -1, -2, -3...etc. in case of collision
            int index = 0;
            var headingId = baseHeadingId;
            var headingBuffer = StringBuilderCache.Local();
            while (!identifiers.Add(headingId))
            {
                index++;
                headingBuffer.Append(baseHeadingId);
                headingBuffer.Append('-');
                headingBuffer.Append(index);
                headingId = headingBuffer.ToString();
                headingBuffer.Length = 0;
            }

            attributes.Id = headingId;
        }
        finally
        {
            OnHeadingParsed?.Invoke(new HeadingInfo(headingBlock.Level, attributes.Id!, headingText));
        }
    }

    private sealed class CacheHtmlRenderer : HtmlRenderer
    {
        public CacheHtmlRenderer() : base(new StringWriter())
        {
            EnableHtmlForInline = false;
            EnableHtmlEscape = false;
        }
        public void ResetThis() => Reset();
    }

    private sealed class StripRendererCache : ObjectCache<CacheHtmlRenderer>
    {
        protected override CacheHtmlRenderer NewInstance() => new CacheHtmlRenderer();
        protected override void Reset(CacheHtmlRenderer instance) => instance!.ResetThis();
    }
}
