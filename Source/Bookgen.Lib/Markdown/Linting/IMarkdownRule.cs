using Bookgen.Lib.Markdown.Linting.Rules;

using Markdig;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Linting;

public interface IMarkdownRule
{
    IEnumerable<LintDiagnostic> Analyze(MarkdownDocument document, string source);
}
