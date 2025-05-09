//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown.TableOfContents;

using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.GenericAttributes;

namespace Bookgen.Lib.Markdown.TableOfContents;

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
