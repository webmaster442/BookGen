//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Text;

namespace BookGen.DomainServices.Markdown.Renderers
{
    internal class SyntaxRenderer : HtmlObjectRenderer<CodeBlock>
    {
        private readonly CodeBlockRenderer _underlyingRenderer;
        private readonly JavaScriptInterop _interop;

        public static bool Enabled { get; set; } = true;

        public SyntaxRenderer(CodeBlockRenderer underlyingRenderer, JavaScriptInterop interop)
        {
            _underlyingRenderer = underlyingRenderer ?? new CodeBlockRenderer();
            _interop = interop;
        }

        private static string GetCode(LeafBlock obj, out string? firstLine)
        {
            var code = new StringBuilder();
            firstLine = null;
            foreach (Markdig.Helpers.StringLine line in obj.Lines.Lines)
            {
                Markdig.Helpers.StringSlice slice = line.Slice;
                if (slice.Text == null)
                    continue;

                string? lineText = slice.Text.Substring(slice.Start, slice.Length);

                if (firstLine == null)
                    firstLine = lineText;
                else
                    code.AppendLine();

                code.Append(lineText);
            }
            return code.ToString();
        }

        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            if (!Enabled
                || !(obj is FencedCodeBlock fencedCodeBlock)
                || !(obj.Parser is FencedCodeBlockParser parser))
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }

            if (string.IsNullOrEmpty(fencedCodeBlock.Info)
                || string.IsNullOrEmpty(parser.InfoPrefix))
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }
            string? languageMoniker = fencedCodeBlock.Info.Replace(parser.InfoPrefix, string.Empty);
            if (string.IsNullOrEmpty(languageMoniker))
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }

            string? code = GetCode(obj, out _);

            string? rendered = Render(code, languageMoniker);

            renderer.Write(rendered);
        }

        private string Render(string code, string languageMoniker)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<pre><code class=\"language-{0}\">\r\n", languageMoniker);
            sb.AppendLine(_interop.SyntaxHighlight(code, languageMoniker));
            sb.AppendLine("</code></pre>");
            return sb.ToString();
        }
    }
}