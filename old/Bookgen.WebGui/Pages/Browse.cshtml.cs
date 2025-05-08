//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.WebGui.Domain;
using BookGen.WebGui.Services;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookGen.WebGui.Pages
{
    public class BrowseModel : PageModel
    {
        private readonly IFileService _files;

        public bool CanPreview(string id) => _files.IsPreviewSupported(id);

        public BrowseModel(IFileService fileService)
        {
            _files = fileService;
        }

        public IList<BrowserItem> Items { get; private set; } = null!;

        public void OnGet(string id)
        {
            Items = _files.GetFiles(id);
        }
    }
}
