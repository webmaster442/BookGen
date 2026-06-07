using System.Collections.Generic;
using System.Threading;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// Interface for all markdown lint rules.
    /// </summary>
    public interface IMarkdownRule
    {
        /// <summary>
        /// Gets the rule information.
        /// </summary>
        RuleInfo Info { get; }

        /// <summary>
        /// Analyzes the document and returns any violations.
        /// </summary>
        /// <param name="analysis">The markdown document analysis.</param>
        /// <param name="configuration">The rule configuration from .editorconfig.</param>
        /// <param name="severity">The severity level to use for violations.</param>
        /// <param name="cancellationToken">Token to cancel analysis early.</param>
        /// <returns>A collection of violations found.</returns>
        IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Configuration options for a rule.
    /// </summary>
    public class RuleConfiguration
    {
        public bool Enabled { get; set; } = true;
        public DiagnosticSeverity Severity { get; set; } = DiagnosticSeverity.Warning;
        public string Value { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = [];

        /// <summary>
        /// Gets or sets the EditorConfig indent_size value for this file, if specified.
        /// This allows rules to use the standard EditorConfig indent setting as a fallback.
        /// </summary>
        public int? EditorConfigIndentSize { get; set; }

        public int GetIntParameter(string name, int defaultValue)
        {
            if (Parameters.TryGetValue(name, out var value) && int.TryParse(value, out var result))
                return result;
            
            if (!string.IsNullOrEmpty(Value) && int.TryParse(Value, out result))
                return result;

            return defaultValue;
        }

        public string GetStringParameter(string name, string defaultValue)
        {
            if (Parameters.TryGetValue(name, out var value))
                return value;
            
            if (!string.IsNullOrEmpty(Value))
                return Value;

            return defaultValue;
        }

        public bool GetBoolParameter(string name, bool defaultValue)
        {
            if (Parameters.TryGetValue(name, out var value))
            {
                if (bool.TryParse(value, out var result))
                    return result;
                return value == "true" || value == "1";
            }
            return defaultValue;
        }
    }
}
