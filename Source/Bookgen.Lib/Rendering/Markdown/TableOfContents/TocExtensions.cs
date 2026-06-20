//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.GenericAttributes;

namespace Bookgen.Lib.Rendering.Markdown.TableOfContents;

internal static class TocExtensions
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
}
