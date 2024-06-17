//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;
using Markdig.Renderers;

namespace BookGen.DomainServices.Markdown.Scripting;

internal sealed class ScriptExtension : IMarkdownExtension
{
    private readonly CsharpScriptExecutor _scriptExecutor;

    public ScriptExtension()
    {
        _scriptExecutor = new CsharpScriptExecutor();
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        pipeline.BlockParsers.AddIfNotAlready(new ScriptBlockParser());
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            htmlRenderer.ObjectRenderers.AddIfNotAlready(new ScriptBlockRenderer(_scriptExecutor));
        }
    }
}
