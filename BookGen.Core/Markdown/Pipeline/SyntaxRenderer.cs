//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Text;

namespace BookGen.Core.Markdown.Pipeline
{
    internal class SyntaxRenderer : HtmlObjectRenderer<CodeBlock>
    {
        private readonly CodeBlockRenderer _underlyingRenderer;

        public SyntaxRenderer(CodeBlockRenderer underlyingRenderer)
        {
            _underlyingRenderer = underlyingRenderer ?? new CodeBlockRenderer();
        }

        private static string GetCode(LeafBlock obj, out string firstLine)
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
            if (!(obj is FencedCodeBlock fencedCodeBlock) || !(obj.Parser is FencedCodeBlockParser parser))
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
            ColorCode.ILanguage lang = FindLanguage(languageMoniker);
            if (lang == null)
                return code;
            else
                return new ColorCode.CodeColorizer().Colorize(code, lang);
        }

        private ColorCode.ILanguage FindLanguage(string languageMoniker)
        {
            switch (languageMoniker.ToLower())
            {
                case "javascript":
                    return ColorCode.Languages.JavaScript;
                case "html":
                    return ColorCode.Languages.Html;
                case "csharp":
                    return ColorCode.Languages.CSharp;
                case "vbdotnet":
                    return ColorCode.Languages.VbDotNet;
                case "ashx":
                    return ColorCode.Languages.Ashx;
                case "asax":
                    return ColorCode.Languages.Asax;
                case "aspx":
                    return ColorCode.Languages.Aspx;
                case "aspxcs":
                    return ColorCode.Languages.AspxCs;
                case "aspxvb":
                    return ColorCode.Languages.AspxVb;
                case "sql":
                    return ColorCode.Languages.Sql;
                case "xml":
                    return ColorCode.Languages.Xml;
                case "php":
                    return ColorCode.Languages.Php;
                case "css":
                    return ColorCode.Languages.Css;
                case "cpp":
                case "c":
                    return ColorCode.Languages.Cpp;
                case "java":
                    return ColorCode.Languages.Java;
                case "powershell":
                    return ColorCode.Languages.PowerShell;
                case "typescript":
                    return ColorCode.Languages.Typescript;
                case "fsharp":
                    return ColorCode.Languages.FSharp;
                case "koka":
                    return ColorCode.Languages.Koka;
                case "haskell":
                    return ColorCode.Languages.Haskell;
                case "markdown":
                    return ColorCode.Languages.Markdown;
                default:
                    return null;
            }
        }
    }
}