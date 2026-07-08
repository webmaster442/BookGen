//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Domain;
using BookGen.Lib.Rendering;

using BookGen.Api.V1;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.PluginInfrastructure.V1;

internal sealed class Document : IDocument
{
    private readonly IReadOnlyFileSystem _fileSystem;
    private readonly ILogger _logger;

    public Document(IReadOnlyFileSystem fileSystem, ILogger logger, string filePath)
    {
        _fileSystem = fileSystem;
        _logger = logger;
        FilePath = filePath;
    }

    public string FilePath { get; }

    public async Task<(string content, IDocumentFrontMatter frontMatter)> ReadContent()
    {
        SourceFile file = await _fileSystem.GetSourceFile(FilePath, _logger);
        return (file.Content, DocumentFrontMatter.CreateFrom(file.FrontMatter));
    }
}
