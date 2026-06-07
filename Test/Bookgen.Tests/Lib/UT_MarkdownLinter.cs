using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Domain.IO.MarkdownLint;
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

    [Test]
    public void EnsureThat_MarkdownLint_Md001_Is_NotDetected()
    {
        const string payload = """
            # Heading 1

            ## Heading 2

            ### Heading 3

            #### Heading 4

            ## Another Heading 2

            ### Another Heading 3
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md001 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md001);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.EqualTo(0));
            Assert.That(md001, Is.Null);
        }
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md003_Is_Detected()
    {
        const string payload = """
            # ATX style H1

            ## Closed ATX style H2 ##

            Setext style H1
            ===============
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md003 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md003);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.GreaterThan(0));
            Assert.That(md003, Is.Not.Null);
        }
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md003_Is_NotDetected()
    {
        const string payload = """
            # ATX style H1

            ## ATX style H2
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md003 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md003);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.EqualTo(0));
            Assert.That(md003, Is.Null);
        }
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md004_Is_Detected()
    {
        const string payload = """
            * Item 1
            + Item 2
            - Item 3
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md004 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md004);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.GreaterThan(0));
            Assert.That(md004, Is.Not.Null);
        }
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md004_Is_NotDetected()
    {
        const string payload = """
            * Item 1
            * Item 2
            * Item 3
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md004 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md004);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.EqualTo(0));
            Assert.That(md004, Is.Null);
        }
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md005_Is_Detected()
    {
        const string payload = """
            * Item 1
            * Nested Item 1
            * Nested Item 2
             * A misaligned item
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md005 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md005);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.GreaterThan(0));
            Assert.That(md005, Is.Not.Null);
        }
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md005_Is_NotDetected()
    {
        const string payload = """
            * Item 1
              * Nested Item 1
              * Nested Item 2
              * Another Nested Item
                * A properly aligned item

            8. Item
            9. Item
            10. Item
            11. Item

            # Left aligneds

             8. Item
             9. Item
            10. Item
            11. Item
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md005 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md005);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.EqualTo(0));
            Assert.That(md005, Is.Null);
        }
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md007_Is_Detected()
    {
        const string payload = """
            * List item
               * Nested list item indented by 3 spaces
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md007 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md007);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.GreaterThan(0));
            Assert.That(md007, Is.Not.Null);
        }
    }

    [Test]
    public void EnsureThat_MarkdownLint_Md007_Is_NotDetected()
    {
        const string payload = """
            * List item
              * Nested list item indented by 2 spaces
            """;

        IReadOnlyList<LintDiagnostic> issues = _sut.Lint(payload);

        LintDiagnostic? md007 = issues
            .FirstOrDefault(d => d.RuleId == LintDiagnostic.Md007);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(issues, Has.Count.EqualTo(0));
            Assert.That(md007, Is.Null);
        }
    }
}
