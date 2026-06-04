//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using Bookgen.Lib.Markdown.Renderers.SyntaxRenderPlugins;
using Bookgen.Lib.Markdown.RenderInterop;

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Renderers;

internal sealed class SyntaxRenderer : HtmlObjectRenderer<CodeBlock>, IDisposable
{
    private readonly CodeBlockRenderer _originalRenderer;
    private readonly IRenderInterop _renderInterop;
    private readonly HashSet<string> _supportedLanguages;
    private readonly Dictionary<string, SyntaxRendererPlugin> _plugins;

    public SyntaxRenderer(CodeBlockRenderer underlyingRenderer,
                          IRenderInterop renderInterop)
    {
        _originalRenderer = underlyingRenderer ?? new CodeBlockRenderer();
        _renderInterop = renderInterop;
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
        _plugins = new Dictionary<string, SyntaxRendererPlugin>();
        RegisterPlugin(new TerminalRenderPlugin());
        RegisterPlugin(new NomnomlRenderPlugin(_renderInterop));
        RegisterPlugin(new LatexRenderPlugin(_renderInterop));
        RegisterPlugin(new QrCodeRenderPlugin(_renderInterop));
        RegisterPlugin(new MermaidRenderPlugin(_renderInterop));
    }

    private void RegisterPlugin(SyntaxRendererPlugin plugin)
    {
        _plugins[plugin.LanguageMoniker] = plugin;
        _supportedLanguages.Add(plugin.LanguageMoniker);
    }

    protected override void Write(HtmlRenderer renderer, CodeBlock obj)
    {
        if (obj is not FencedCodeBlock fencedCodeBlock
            || obj.Parser is not FencedCodeBlockParser parser
            || string.IsNullOrEmpty(fencedCodeBlock.Info)
            || string.IsNullOrEmpty(parser.InfoPrefix))
        {
            _originalRenderer.Write(renderer, obj);
            return;
        }

        string languageMoniker = fencedCodeBlock.Info.Replace(parser.InfoPrefix, string.Empty);


        if (string.IsNullOrEmpty(languageMoniker)
            || !_supportedLanguages.Contains(languageMoniker))
        {
            _originalRenderer.Write(renderer, obj);
            return;
        }

        string code = obj.GetCode();

        if (_plugins.TryGetValue(languageMoniker, out SyntaxRendererPlugin? plugin))
        {
            renderer.Write(plugin.Render(code));
            return;
        }

        renderer.Write(RenderWithPrism(code, languageMoniker));
    }

    private string RenderWithPrism(string code, string languageMoniker)
    {
        var sb = new StringBuilder();
        sb.AppendFormat("<pre><code class=\"language-{0}\">", languageMoniker);
        sb.AppendLine(_renderInterop?.PrismSyntaxHighlight(code, languageMoniker));
        sb.AppendLine("</code></pre>");
        return sb.ToString();
    }

    public void Dispose()
    {
        _renderInterop.Dispose();
    }
}
