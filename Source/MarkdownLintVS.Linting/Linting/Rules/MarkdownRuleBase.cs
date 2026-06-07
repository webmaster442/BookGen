using System.Collections.Generic;
using System.Threading;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// Base class for markdown lint rules providing common functionality.
    /// </summary>
    public abstract class MarkdownRuleBase : IMarkdownRule
    {
        public abstract RuleInfo Info { get; }

        public abstract IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default);

        protected LintViolation CreateViolation(
            int lineNumber,
            int columnStart,
            int columnEnd,
            string message,
            DiagnosticSeverity severity,
            string fixDescription = null)
        {
            return new LintViolation(
                Info,
                lineNumber,
                columnStart,
                columnEnd,
                message,
                severity,
                fixDescription);
        }

        protected LintViolation CreateLineViolation(
            int lineNumber,
            string line,
            string message,
            DiagnosticSeverity severity,
            string? fixDescription = null)
        {
            return new LintViolation(
                Info,
                lineNumber,
                0,
                line?.Length ?? 0,
                message,
                severity,
                fixDescription);
        }
    }
}
