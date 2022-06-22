//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Text;

namespace BookGen.Core.Markdown.Renderers
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
            foreach (var line in obj.Lines.Lines)
            {
                var slice = line.Slice;
                if (slice.Text == null)
                    continue;

                var lineText = slice.Text.Substring(slice.Start, slice.Length);

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
            var languageMoniker = fencedCodeBlock.Info.Replace(parser.InfoPrefix, string.Empty);
            if (string.IsNullOrEmpty(languageMoniker))
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }

            var code = GetCode(obj, out _);

            var rendered = Render(code, languageMoniker);

            renderer.Write(rendered);
        }

        private string Render(string code, string languageMoniker)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<pre><code class=\"language-{0}\">\r\n", languageMoniker);
            sb.AppendLine(_interop.SyntaxHighlight(code, languageMoniker));
            sb.AppendLine("</code></pre>");
            return sb.ToString();
        }
    }
}