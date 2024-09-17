//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.WebGui.Services;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookGen.WebGui.Pages;

public class DocumentModel : PageModel
{
    private readonly IDocumentProvider _documentProvider;

    public DocumentModel(IDocumentProvider documentProvider)
    {
        _documentProvider = documentProvider;
        DocumentContent = string.Empty;
    }

    public string DocumentContent { get; private set; }

    public void OnGet(Document doc)
    {
        var content = _documentProvider.GetDocument(doc);
        ViewData["Title"] = doc.ToString();
        DocumentContent = content;
    }
}
