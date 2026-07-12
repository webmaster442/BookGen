//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Rendering.Markdown.Keyboard;
using BookGen.Lib.Rendering.Markdown.TableOfContents;

using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.GenericAttributes;
using Markdig.Helpers;

namespace Bookgen.Lib.Rendering.Markdown;

internal static class PipelineBuilderExtensions
{
    public static MarkdownPipelineBuilder UseTableOfContents(this MarkdownPipelineBuilder pipelineBuilder)
    {
        var state = new TocState();
        var autoIdOptons = new CustomAutoIdOptions();

        pipelineBuilder.Extensions.ReplaceOrAdd<AutoIdentifierExtension>(new CustomAutoIdExtension(autoIdOptons));

        var tocExtension = new TocExtension(state);

        if (pipelineBuilder.Extensions.Find<GenericAttributesExtension>() is not null)
            pipelineBuilder.Extensions.InsertBefore<GenericAttributesExtension>(tocExtension);
        else
            pipelineBuilder.Extensions.AddIfNotAlready(tocExtension);

        return pipelineBuilder;
    }

    public static MarkdownPipelineBuilder UseKeyboard(this MarkdownPipelineBuilder pipeline)
    {
        OrderedList<IMarkdownExtension> extensions = pipeline.Extensions;

        if (!extensions.Contains<KeyboardExtension>())
        {
            extensions.Add(new KeyboardExtension());
        }

        return pipeline;
    }
}
