using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Linting.Rules;

internal sealed class Md001 : IMarkdownRule
{
    public IEnumerable<LintDiagnostic> Analyze(MarkdownDocument document, string source)
    {
        int? previousLevel = null;

        foreach (HeadingBlock heading in document.AllBlocks<HeadingBlock>())
        {
            if (previousLevel != null &&
                heading.Level > previousLevel + 1)
            {
                yield return new LintDiagnostic
                {
                    RuleId = "MD001",
                    RuleName = "Heading levels should only increment by one level at a time",
                    HelpUri = new Uri("https://github.com/DavidAnson/markdownlint/blob/main/doc/md001.md"),
                    Column = heading.Column + 1,
                    Line = heading.Line + 1,
                    Message = $"Heading level jumps from H{previousLevel} to H{heading.Level}"
                };
            }

            previousLevel = heading.Level;
        }
    }
}
