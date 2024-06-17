//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;
using Markdig.Renderers;

namespace BookGen.DomainServices.Markdown.Scripting;

internal static class ScriptExtensions
{
    public static MarkdownPipelineBuilder UseScripting(this MarkdownPipelineBuilder pipelineBuilder)
    {
        pipelineBuilder.Extensions.AddIfNotAlready(new ScriptExtension());
        return pipelineBuilder;
    }
}
