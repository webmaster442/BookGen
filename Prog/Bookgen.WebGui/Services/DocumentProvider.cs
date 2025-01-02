//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;
using BookGen.Interfaces;
using BookGen.WebGui.Domain;


namespace BookGen.WebGui.Services;

internal sealed class DocumentProvider : IDocumentProvider
{
    private readonly ILogger<DocumentProvider> _logger;
    private readonly ICurrentSession _currentSession;

    public DocumentProvider(ILogger<DocumentProvider> logger, ICurrentSession currentSession)
    {
        _logger = logger;
        _currentSession = currentSession;
    }

    private string GetFileConentents(string file)
    {
        FsPath path = _currentSession.AppDirectory.Combine(file);
        if (!path.IsExisting)
        {
            return $"<h1 class=\"text-danger\">File not found - {file}</h1>";
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
