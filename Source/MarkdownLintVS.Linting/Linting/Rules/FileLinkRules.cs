using Markdig.Syntax.Inlines;

namespace MarkdownLintVS.Linting.Rules
{
    /// <summary>
    /// MD061: File links should reference existing files.
    /// Validates that relative links to local files point to files that actually exist.
    /// Supports root-relative paths (starting with /) when root_path is configured.
    /// </summary>
    public class MD061_FileLinkExists : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD061;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(analysis.FilePath))
                yield break;

            var baseDirectory = Path.GetDirectoryName(analysis.FilePath);
            if (string.IsNullOrEmpty(baseDirectory))
                yield break;

            var rootPath = analysis.RootPath;

            foreach (LinkInline link in analysis.GetLinks())
            {
                // Skip image links - those are handled by MD062
                if (link.IsImage)
                    continue;

                var url = link.Url;
                if (string.IsNullOrEmpty(url))
                    continue;

                // Skip external URLs
                if (IsExternalUrl(url))
                    continue;

                // Skip data URLs and javascript
                if (url.StartsWith("data:", StringComparison.OrdinalIgnoreCase) ||
                    url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Check if the local file exists
                if (!LocalFileExists(url, baseDirectory, rootPath))
                {
                    (var line, var column) = analysis.GetPositionFromOffset(link.Span.Start);
                    var cleanUrl = GetPathWithoutFragment(url);

                    yield return CreateViolation(
                        line,
                        column,
                        column + link.Span.Length,
                        $"Link references non-existent file: '{cleanUrl}'",
                        severity);
                }
            }
        }

        private static bool IsExternalUrl(string url)
        {
            return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("tel:", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("//", StringComparison.Ordinal);
        }

        private static string GetPathWithoutFragment(string url)
        {
            var fragmentIndex = url.IndexOf('#');
            return fragmentIndex >= 0 ? url.Substring(0, fragmentIndex) : url;
        }

        /// <summary>
        /// Checks if a local file exists, using the same path resolution logic as MarkdownEditor2022.
        /// </summary>
        /// <param name="url">The URL/path from the link.</param>
        /// <param name="baseDirectory">The directory containing the markdown file.</param>
        /// <param name="rootPath">Optional root path for resolving root-relative paths (starting with /).</param>
        private static bool LocalFileExists(string url, string baseDirectory, string rootPath)
        {
            try
            {
                // Remove fragment
                var path = GetPathWithoutFragment(url);

                // If only a fragment (e.g., "#section"), it references the current file
                if (string.IsNullOrEmpty(path))
                    return true;

                // URL decode the path
                path = Uri.UnescapeDataString(path);

                // Remove query string if present
                var queryIndex = path.IndexOf('?');
                if (queryIndex >= 0)
                    path = path.Substring(0, queryIndex);

                string fullPath;

                // Check if this is a root-relative path (starts with /)
                if (path.StartsWith("/", StringComparison.Ordinal))
                {
                    // Root-relative paths require a root_path to be configured
                    if (string.IsNullOrEmpty(rootPath))
                    {
                        // No root path configured - cannot resolve root-relative paths
                        // Fall back to treating it as relative to base directory
                        fullPath = Path.GetFullPath(Path.Combine(baseDirectory, path.TrimStart('/')));
                    }
                    else
                    {
                        // Remove leading slash and normalize path separators
                        var pathWithoutLeadingSlash = path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);

                        // Resolve against the root path
                        fullPath = Path.GetFullPath(Path.Combine(rootPath, pathWithoutLeadingSlash));
                    }
                }
                else
                {
                    // Regular relative path - resolve against base directory
                    fullPath = Path.GetFullPath(Path.Combine(baseDirectory, path));
                }

                // Check if it's a file or directory
                return File.Exists(fullPath) || Directory.Exists(fullPath);
            }
            catch
            {
                // If path is malformed, consider it as non-existent
                return false;
            }
        }
    }

    /// <summary>
    /// MD062: Image links should reference existing files.
    /// Validates that relative image links point to files that actually exist.
    /// Supports root-relative paths (starting with /) when root_path is configured.
    /// </summary>
    public class MD062_ImageLinkExists : MarkdownRuleBase
    {
        private static readonly RuleInfo _info = RuleRegistry.MD062;
        public override RuleInfo Info => _info;

        public override IEnumerable<LintViolation> Analyze(
            MarkdownDocumentAnalysis analysis,
            RuleConfiguration configuration,
            DiagnosticSeverity severity,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(analysis.FilePath))
                yield break;

            var baseDirectory = Path.GetDirectoryName(analysis.FilePath);
            if (string.IsNullOrEmpty(baseDirectory))
                yield break;

            var rootPath = analysis.RootPath;

            foreach (LinkInline link in analysis.GetLinks())
            {
                // Only process image links
                if (!link.IsImage)
                    continue;

                var url = link.Url;
                if (string.IsNullOrEmpty(url))
                    continue;

                // Skip external URLs
                if (IsExternalUrl(url))
                    continue;

                // Skip data URLs (embedded images)
                if (url.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Check if the local file exists
                if (!LocalFileExists(url, baseDirectory, rootPath))
                {
                    (var line, var column) = analysis.GetPositionFromOffset(link.Span.Start);

                    yield return CreateViolation(
                        line,
                        column,
                        column + link.Span.Length,
                        $"Image references non-existent file: '{url}'",
                        severity);
                }
            }
        }

        private static bool IsExternalUrl(string url)
        {
            return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("//", StringComparison.Ordinal);
        }

        /// <summary>
        /// Checks if a local image file exists, using the same path resolution logic as MarkdownEditor2022.
        /// </summary>
        /// <param name="url">The URL/path from the image link.</param>
        /// <param name="baseDirectory">The directory containing the markdown file.</param>
        /// <param name="rootPath">Optional root path for resolving root-relative paths (starting with /).</param>
        private static bool LocalFileExists(string url, string baseDirectory, string rootPath)
        {
            try
            {
                // URL decode the path
                var path = Uri.UnescapeDataString(url);

                // Remove query string if present
                var queryIndex = path.IndexOf('?');
                if (queryIndex >= 0)
                    path = path.Substring(0, queryIndex);

                string fullPath;

                // Check if this is a root-relative path (starts with /)
                if (path.StartsWith("/", StringComparison.Ordinal))
                {
                    // Root-relative paths require a root_path to be configured
                    if (string.IsNullOrEmpty(rootPath))
                    {
                        // No root path configured - cannot resolve root-relative paths
                        // Fall back to treating it as relative to base directory
                        fullPath = Path.GetFullPath(Path.Combine(baseDirectory, path.TrimStart('/')));
                    }
                    else
                    {
                        // Remove leading slash and normalize path separators
                        var pathWithoutLeadingSlash = path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);

                        // Resolve against the root path
                        fullPath = Path.GetFullPath(Path.Combine(rootPath, pathWithoutLeadingSlash));
                    }
                }
                else
                {
                    // Regular relative path - resolve against base directory
                    fullPath = Path.GetFullPath(Path.Combine(baseDirectory, path));
                }

                return File.Exists(fullPath);
            }
            catch
            {
                // If path is malformed, consider it as non-existent
                return false;
            }
        }
    }
}
