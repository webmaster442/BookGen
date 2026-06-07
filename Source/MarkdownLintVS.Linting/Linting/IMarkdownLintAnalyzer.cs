using System.Collections.Generic;
using System.Threading;
using MarkdownLintVS.Linting.Rules;

namespace MarkdownLintVS.Linting
{
    /// <summary>
    /// Interface for markdown lint analysis services.
    /// Enables dependency injection and unit testing without VS dependencies.
    /// </summary>
    public interface IMarkdownLintAnalyzer
    {
        /// <summary>
        /// Analyzes a markdown document and returns all violations.
        /// </summary>
        /// <param name="text">The markdown text to analyze.</param>
        /// <param name="filePath">The file path (used for EditorConfig resolution).</param>
        /// <param name="cancellationToken">Token to cancel analysis early.</param>
        /// <returns>A collection of lint violations found in the document.</returns>
        IEnumerable<LintViolation> Analyze(string text, string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the list of all registered rules.
        /// </summary>
        IReadOnlyList<IMarkdownRule> Rules { get; }
    }
}
