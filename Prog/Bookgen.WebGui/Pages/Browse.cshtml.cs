using BookGen.WebGui.Domain;
using BookGen.WebGui.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookGen.WebGui.Pages
{
    public class BrowseModel : PageModel
    {
        private readonly IFileItemProvider _itemprovider;
        private readonly ICurrentSession _currentSession;

        public BrowseModel(IFileItemProvider fileItemProvider, ICurrentSession currentSession)
        {
            _itemprovider = fileItemProvider;
            _currentSession = currentSession;
        }

        public IList<BrowserItem> Items { get; private set; } = null!;

        public void OnGet(string id)
        {
            Items = _itemprovider.GetFiles(id);
        }
    }
}
