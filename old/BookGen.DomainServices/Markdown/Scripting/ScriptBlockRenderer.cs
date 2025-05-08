//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace BookGen.DomainServices.Markdown.Scripting;

internal sealed class ScriptBlockRenderer : HtmlObjectRenderer<ScriptBlock>
{
    private readonly CsharpScriptExecutor _scriptExecutor;

    public ScriptBlockRenderer(CsharpScriptExecutor scriptExecutor)
    {
        _scriptExecutor = scriptExecutor;
    }

    protected override void Write(HtmlRenderer renderer, ScriptBlock obj)
    {
        var result = _scriptExecutor.Execute(obj.GetScript()).Result;
        renderer.Write(result);
    }
}
