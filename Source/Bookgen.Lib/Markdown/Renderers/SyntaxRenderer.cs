using System.Text;
using System.Web;

using Bookgen.Lib.JsInterop;

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Renderers;

internal sealed class SyntaxRenderer : HtmlObjectRenderer<CodeBlock>, IDisposable
{
    private readonly CodeBlockRenderer _originalRenderer;
    private readonly PrismJsInterop? _prism;
    private readonly HashSet<string> _supportedLanguages;
    public const string Terminallanguage = "terminal";

    public const string TerminalHtml = """
        <div style="margin-bottom: 1rem;" class="terminaloutput">
        <div style="background-color: #877EC2; color: #eee8d6; padding: 3px;">$</div>
        <pre style="text-align: left; font-size: 1.1em; line-height: 1.5; margin: 0px; padding: 8px; background-color: #282c34; color: #DCDFE4; font-family: Monaco, Menlo, Consolas, 'Courier New', monospace; word-break: break-all; word-wrap: break-word; overflow: auto;"><code style="tab-size: 4;"><!--{Code}--></code></pre>
        </div>
        """;

    public bool PreRender => _prism != null;

    public SyntaxRenderer(CodeBlockRenderer underlyingRenderer, PrismJsInterop? prism)
    {
        _originalRenderer = underlyingRenderer ?? new CodeBlockRenderer();
        _prism = prism;
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
                "xeora", "xojo", "xquery", "yaml", Terminallanguage
            };
    }

    public static string GetCode(LeafBlock node)
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

        string code = GetCode(obj);

        if (languageMoniker == Terminallanguage)
        {
            string terminalOutput = RenderTerminalString(code);
            renderer.Write(terminalOutput);
        }
        else if (PreRender)
        {
            string rendered = RenderWithPrism(code, languageMoniker);
            renderer.Write(rendered);
        }
        else
        {
            _originalRenderer.Write(renderer, obj);
        }
    }

    public static string RenderTerminalString(string code)
    {
        const string codeTag = "<!--{Code}-->";
        return TerminalHtml.Replace(codeTag, HttpUtility.HtmlEncode(code));

    }

    private string RenderWithPrism(string code, string languageMoniker)
    {
        var sb = new StringBuilder();
        sb.AppendFormat("<pre><code class=\"language-{0}\">", languageMoniker);
        sb.AppendLine(_prism?.PrismSyntaxHighlight(code, languageMoniker));
        sb.AppendLine("</code></pre>");
        return sb.ToString();
    }

    public void Dispose()
    {
        _prism?.Dispose();
    }
}
