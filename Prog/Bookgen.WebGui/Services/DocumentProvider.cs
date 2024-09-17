//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;
using BookGen.Interfaces;


namespace BookGen.WebGui.Services;

internal sealed class DocumentProvider : IDocumentProvider
{
    private readonly ILogger _logger;
    private readonly ICurrentSession _currentSession;

    public DocumentProvider(ILoggerFactory logger, ICurrentSession currentSession)
    {
        _logger = logger.CreateLogger(nameof(DocumentProvider));
        _currentSession = currentSession;
    }

    private string GetFileConentents(string file)
    {
        FsPath path = _currentSession.AppDirectory.Combine(file);
        if (!path.IsExisting)
        {
            return $"File not found - {file}";
        }
        return path.ReadFile(_logger);
    }

    public string GetDocument(Document document)
    {
        return document switch
        {
            Document.Commands => GetFileConentents("Commands.html"),
            Document.ChangeLog => GetFileConentents("ChangeLog.html"),
            Document.ReleaseNotes => GetFileConentents("RelaseNotes.html"),
            Document.MarkdownCheatsheet => GetFileConentents("Markdown-cheatsheet.html"),
            _ => "Unknown document",
        };
    }
}
