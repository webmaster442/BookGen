//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;
using BookGen.WebGui.Services;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookGen.WebGui.Pages;

public class PreviewModel : PageModel
{
    private readonly IFileService _fileService;

    public PreviewModel(IFileService fileService)
    {
        _fileService = fileService;
    }

    public bool IsMarkdown { get; set; }

    public string Text { get; set; } = "";

    public string FileName { get; set; } = "";

    public void OnGet(string id)
    {
        if (!_fileService.IsPreviewSupported(id))
        {
            Response.Redirect($"/Raw?id={id}");
            return;
        }

        FileName = _fileService.GetFileNameOf(id);

        if (_fileService.IsMarkdown(id))
        {
            IsMarkdown = true;
            using var pipeline = new BookGenPipeline(BookGenPipeline.Preview);
            pipeline.InjectPath(_fileService.GetDirectoryOf(id));
            Text = pipeline.RenderMarkdown(_fileService.GetTextContent(id));
            return;
        }

        
        Text = _fileService.GetTextContent(id);
    }
}
