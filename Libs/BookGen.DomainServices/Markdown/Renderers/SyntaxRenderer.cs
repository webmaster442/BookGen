//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
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
        private readonly HashSet<string> _supportedLanguages;

        public static bool Enabled { get; set; } = true;

        public SyntaxRenderer(CodeBlockRenderer underlyingRenderer, JavaScriptInterop interop)
        {
            _underlyingRenderer = underlyingRenderer ?? new CodeBlockRenderer();
            _interop = interop;
            _supportedLanguages = new HashSet<string>
            {
                "markup", "css", "clike", "javascript", "abap", "actionscript",
                "ada", "apacheconf", "apl", "applescript", "arduino", "arff",
                "asciidoc", "asm6502", "aspnet", "autohotkey", "autoit", "bash", 
                "basic", "batch", "bison", "brainfuck", "bro", "c", "csharp", 
                "cpp", "coffeescript", "clojure", "crystal", "csp", "css-extras",
                "d", "dart", "diff", "django", "docker", "eiffel", "elixir", "elm",
                "erb", "erlang", "fsharp", "flow", "fortran", "gedcom", "gherkin",
                "git", "glsl", "gml", "go", "graphql", "groovy", "haml", "handlebars",
                "haskell", "haxe", "http", "hpkp", "hsts", "ichigojam", "icon",
                "inform7", "ini", "io", "j", "java", "jolie", "json", "julia", "keyman", 
                "kotlin", "latex", "less", "liquid", "lisp", "livescript", "lolcode", "lua",
                "makefile", "markdown", "markup-templating", "matlab", "mel", "mizar", 
                "monkey", "n4js", "nasm", "nginx", "nim", "nix", "nsis", "objectivec",
                "ocaml", "opencl", "oz", "parigp", "parser", "pascal", "perl", "php",
                "php-extras", "plsql", "powershell", "processing", "prolog", "properties",
                "protobuf", "pug", "puppet", "pure", "python", "q", "qore", "r", "jsx", 
                "tsx", "renpy", "reason", "rest", "rip", "roboconf", "ruby", "rust", "sas",
                "sass", "scss", "scala", "scheme", "smalltalk", "smarty", "sql", "soy", 
                "stylus", "swift", "tap", "tcl", "textile", "tt2", "twig", "typescript",
                "vbnet", "velocity", "verilog", "vhdl", "vim", "visual-basic", "wasm", "wiki",
                "xeora", "xojo", "xquery", "yaml"
            };
        }

        private static string GetCode(LeafBlock node)
        {
            var code = new StringBuilder();
            var lines = node.Lines.Lines;
            int totalLines = lines.Length;
            for (int i = 0; i < totalLines; i++)
            {
                var line = lines[i];
                var slice = line.Slice;
                if (slice.Text == null)
                {
                    continue;
                }

                var lineText = slice.Text.Substring(slice.Start, slice.Length);
                if (i > 0)
                {
                    code.AppendLine();
                }

                code.Append(lineText);
            }

            return code.ToString();
        }

        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            if (!Enabled
                || obj is not FencedCodeBlock fencedCodeBlock
                || obj.Parser is not FencedCodeBlockParser parser)
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
            string languageMoniker = fencedCodeBlock.Info.Replace(parser.InfoPrefix, string.Empty);
            if (string.IsNullOrEmpty(languageMoniker) 
                || !_supportedLanguages.Contains(languageMoniker))
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }

            string code = GetCode(obj);

            string rendered = Render(code, languageMoniker);

            renderer.Write(rendered);
        }

        private string Render(string code, string languageMoniker)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<pre><code class=\"language-{0}\">", languageMoniker);
            sb.AppendLine();
            sb.AppendLine(_interop.SyntaxHighlight(code, languageMoniker));
            sb.AppendLine("</code></pre>");
            return sb.ToString();
        }
    }
}