using Bookgen.Lib.JsInterop;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class GeneratePager : PipeLineStep<StaticWebState>
{
    public GeneratePager(StaticWebState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Generating pager...");
        var jsBuilder = new JavascriptBuilder();

        var script = jsBuilder
            .DeclareArray("urls", State.TocLinks)
            .DeclareConst("currentUrl", "window.location.origin + window.location.pathname")
            .DeclareConst("index", "urls.indexOf(currentUrl)")
            .DocumentWriteLine("<div class=\"pager\">")
            .Block("""     
            if (index > 0) {
            
                document.writeln(`<a href="${urls[index - 1]}" class="prev">⬅️</a>`);
            }

            if (index !== -1 && index + 1 < urls.length) {
            
                document.writeln(`<a href="${urls[index + 1]}" class="next">➡️</a>`);
            }
            """)
            .DocumentWriteLine("</div>")
            .ToString();

        await environment.Output.WriteAllTextAsync("pager.js", script);

        return StepResult.Success;
    }
}
