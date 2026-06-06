namespace Bookgen.Lib.Markdown.Linting;

public sealed record class LintDiagnostic
{
    public required string RuleId { get; init; }
    public required string Message { get; init; }
    public required Uri HelpUri { get; init; }
    public required int Line { get; init; }
    public required int Column { get; init; }
    public required string RuleName { get; init; }

    public const string Md001 = "MD001";
}
