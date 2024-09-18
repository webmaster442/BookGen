using BookGen.WebGui.Domain;
using BookGen.WebGui.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookGen.WebGui.Pages
{
    public class BrowseModel : PageModel
    {
        private readonly IFileService _fileService;
        private readonly ICurrentSession _currentSession;

        public BrowseModel(IFileService fileService, ICurrentSession currentSession)
        {
            _fileService = fileService;
            _currentSession = currentSession;
            Items = _fileService.GetFiles(_currentSession.StartDirectory.ToString());
        }

        public IList<BrowserItem> Items { get; }

        public void OnGet()
        {

        }
    }
}
