using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Markdown.Linting;

namespace Bookgen.Tests.Lib;

[TestFixture]
internal class UT_MarkdownLinter
{
    private MarkdownLinter _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new MarkdownLinter(new MarkdownLintSettings());
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md001_Is_Detected()
    {
        const string payload = """
            # Heading 1

            ### Heading 3

            We skipped out a 2nd level heading in this document
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md001 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md001);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.GreaterThan(0));
            Assert.That(md001, Is.Not.Null);
        }
    }
}
