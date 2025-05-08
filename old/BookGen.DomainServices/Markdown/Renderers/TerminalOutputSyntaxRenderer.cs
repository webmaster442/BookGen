//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace BookGen.DomainServices.Markdown.Renderers;
internal sealed class TerminalOutputSyntaxRenderer : HtmlObjectRenderer<CodeBlock>
{
    private readonly CodeBlockRenderer _originalRenderer;

    public TerminalOutputSyntaxRenderer(CodeBlockRenderer originalRenderer)
    {
        _originalRenderer = originalRenderer;
    }

    protected override void Write(HtmlRenderer renderer, CodeBlock obj)
    {
        if (obj is not FencedCodeBlock fencedCodeBlock
            || obj.Parser is not FencedCodeBlockParser parser)
        {
            _originalRenderer.Write(renderer, obj);
            return;
        }

        if (string.IsNullOrEmpty(fencedCodeBlock.Info)
            || string.IsNullOrEmpty(parser.InfoPrefix))
        {
            _originalRenderer.Write(renderer, obj);
            return;
        }

        string languageMoniker = fencedCodeBlock.Info.Replace(parser.InfoPrefix, string.Empty);

        if (languageMoniker == SyntaxRenderer.Terminallanguage)
        {
            string code = SyntaxRenderer.GetCode(obj);
            string terminalOutput = SyntaxRenderer.RenderTerminalString(code);
            renderer.Write(terminalOutput);
        }
        else
        {
            _originalRenderer.Write(renderer, obj);
        }
    }
}
