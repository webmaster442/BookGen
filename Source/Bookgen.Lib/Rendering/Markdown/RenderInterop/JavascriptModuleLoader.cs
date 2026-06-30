//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using BookGen.Vfs;

using Microsoft.ClearScript;

namespace Bookgen.Lib.Rendering.Markdown.RenderInterop;

internal sealed class JavascriptModuleLoader : DocumentLoader
{
    private static readonly IReadOnlyCollection<string> relativePrefixes =
    [
        "." + Path.DirectorySeparatorChar,
        "." + Path.AltDirectorySeparatorChar,
        ".." + Path.DirectorySeparatorChar,
        ".." + Path.AltDirectorySeparatorChar,
    ];

    private readonly IAssetSource? _moduleAssets;

    public JavascriptModuleLoader(IAssetSource? moduleAssets)
    {
        _moduleAssets = moduleAssets;
    }

    public override async Task<Document> LoadDocumentAsync(DocumentSettings settings,
                                                           DocumentInfo? sourceInfo,
                                                           string specifier,
                                                           DocumentCategory category,
                                                           DocumentContextCallback contextCallback)
    {
        if (settings.SearchPath != "/")
            throw new InvalidOperationException("Search path is not configured correctly for Javascript module loading.");

        category ??= sourceInfo.HasValue ? sourceInfo.Value.Category : DocumentCategory.Script;

        List<Uri> CandidateUris = Uri.TryCreate(specifier, UriKind.RelativeOrAbsolute, out Uri? uri) && uri.IsAbsoluteUri
            ? GetCandidateUris(settings, sourceInfo, [uri])
            : GetCandidateUris(settings, sourceInfo, GetRawUris(settings, sourceInfo, specifier).Distinct());

        if (CandidateUris.Count < 1)
            throw new FileNotFoundException(null, specifier);

        if (CandidateUris.Count == 1)
        {
            return await LoadDocumentAsync(CandidateUris[0], category, contextCallback);
        }

        var exceptions = new List<Exception>(CandidateUris.Count);

        foreach (Uri candidateUri in CandidateUris)
        {
            Task<Document> task = LoadDocumentAsync(candidateUri, category, contextCallback);
            try
            {
                return await task;
            }
            catch (Exception exception)
            {
                if (task.Exception?.InnerExceptions.Count == 1)
                {
                    exceptions.Add(task.Exception);
                }
                else
                {
                    exceptions.Add(exception);
                }
            }
        }

        if (exceptions.Count < 1)
        {
            throw new FileNotFoundException(null, specifier);
        }

        if (exceptions.Count == 1)
        {
            throw new FileLoadException(exceptions[0].Message, specifier, exceptions[0]);
        }

        throw new AggregateException(exceptions).Flatten();
    }

    private static Uri? GetBaseUri(DocumentInfo sourceInfo)
    {
        Uri? sourceUri = sourceInfo.Uri;

        if ((sourceUri is null) && !Uri.TryCreate(sourceInfo.Name, UriKind.RelativeOrAbsolute, out sourceUri))
        {
            return null;
        }

        return !sourceUri.IsAbsoluteUri
            ? null 
            : sourceUri;
    }

    private static IEnumerable<string> SplitSearchPath(string searchPath)
    {
        return searchPath.Split(';', StringSplitOptions.RemoveEmptyEntries).Distinct(StringComparer.OrdinalIgnoreCase);
    }

    private static bool TryCombineSearchUri(Uri searchUri, string specifier, [NotNullWhen(true)] out Uri? uri)
    {
        var searchUrl = searchUri.AbsoluteUri;
        if (!searchUrl.EndsWith('/'))
        {
            searchUri = new Uri(searchUrl + "/");
        }

        return Uri.TryCreate(searchUri, specifier, out uri);
    }

    private static IEnumerable<Uri> GetRawUris(DocumentSettings settings, DocumentInfo? sourceInfo, string specifier)
    {
        Uri? baseUri;
        Uri? uri;

        if (sourceInfo.HasValue && relativePrefixes.Any(specifier.StartsWith))
        {
            baseUri = GetBaseUri(sourceInfo.Value);
            if ((baseUri is not null) && Uri.TryCreate(baseUri, specifier, out uri))
            {
                yield return uri;
            }
        }

        var searchPath = settings.SearchPath;
        if (!string.IsNullOrWhiteSpace(searchPath))
        {
            foreach (var url in SplitSearchPath(searchPath))
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out baseUri)
                    && TryCombineSearchUri(baseUri, specifier, out uri))
                {
                    yield return uri;
                }
            }
        }
    }

    private static IEnumerable<string> GetCompatibleExtensions(DocumentInfo? sourceInfo, string extensions)
    {
        string? sourceExtension = null;

        if (sourceInfo.HasValue)
        {
            sourceExtension = Path.GetExtension((sourceInfo.Value.Uri is not null) ? new UriBuilder(sourceInfo.Value.Uri).Path : sourceInfo.Value.Name);
            if (!string.IsNullOrEmpty(sourceExtension))
            {
                yield return sourceExtension;
            }
        }

        foreach (var extension in SplitSearchPath(extensions))
        {
            var tempExtension = extension.StartsWith(".", StringComparison.Ordinal) ? extension : "." + extension;
            if (!tempExtension.Equals(sourceExtension, StringComparison.OrdinalIgnoreCase))
            {
                yield return tempExtension;
            }
        }
    }

    private static IEnumerable<Uri> ApplyExtensions(DocumentInfo? sourceInfo, Uri uri, string extensions)
    {
        yield return uri;

        var builder = new UriBuilder(uri);
        var path = builder.Path;

        if (!string.IsNullOrEmpty(Path.GetFileName(path)))
        {
            var existingExtension = Path.GetExtension(path);
            var compatibleExtensions = GetCompatibleExtensions(sourceInfo, extensions).ToList();

            if (!compatibleExtensions.Contains(existingExtension, StringComparer.OrdinalIgnoreCase))
            {
                foreach (var compatibleExtension in compatibleExtensions)
                {
                    builder.Path = Path.ChangeExtension(path, existingExtension + compatibleExtension);
                    yield return builder.Uri;
                }
            }
        }
    }

    private static List<Uri> GetCandidateUris(DocumentSettings settings, DocumentInfo? sourceInfo, IEnumerable<Uri> rawUris)
    {
        if (!string.IsNullOrWhiteSpace(settings.FileNameExtensions))
        {
            rawUris = rawUris.SelectMany(uri => ApplyExtensions(sourceInfo, uri, settings.FileNameExtensions));
        }

        var testUris = rawUris.ToList();
        var candidateUris = new List<Uri>();

        foreach (Uri? testUri in testUris)
        {
            if (File.Exists(testUri.LocalPath))
            {
                candidateUris.Add(testUri);
            }
        }

        return candidateUris;
    }


    private async Task<Document> LoadDocumentAsync(Uri uri,
                                                   DocumentCategory category,
                                                   DocumentContextCallback contextCallback)
    {
        if (_moduleAssets  == null)
        {
            throw new InvalidOperationException("Module assets source is not provided.");
        }

        if (!uri.IsFile)
        {
            throw new NotSupportedException("Only file URIs are supported.");
        }

        string contents = _moduleAssets.GetAsset(uri.LocalPath);

        var documentInfo = new DocumentInfo(uri) { Category = category, ContextCallback = contextCallback };

        return new StringDocument(documentInfo, contents);
    }
}
