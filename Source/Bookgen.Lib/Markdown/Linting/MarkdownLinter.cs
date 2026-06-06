using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Markdown.Linting.Rules;

using Markdig;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Linting;

public sealed class MarkdownLinter
{
    private readonly IMarkdownRule[] _rules;
    private readonly MarkdownPipeline _lintPipeline;

    private static IEnumerable<IMarkdownRule> LoadRules(MarkdownLintSettings settings)
    {
        if (settings.Md001)
            yield return new Md001();
    }

    public MarkdownLinter(MarkdownLintSettings settings)
    {
        _rules = LoadRules(settings).ToArray();
        _lintPipeline = new MarkdownPipelineBuilder()
            .UsePreciseSourceLocation()
            .Build();
    }

    public IReadOnlyList<LintDiagnostic> Lint(string document)
    {
        MarkdownDocument markdownDocument = Markdig.Markdown.Parse(document, _lintPipeline);
        List<LintDiagnostic> diagnostics = new();
        foreach (IMarkdownRule rule in _rules)
        {
            diagnostics.AddRange(rule.Analyze(markdownDocument, document));
        }
        return diagnostics;
    }
}
