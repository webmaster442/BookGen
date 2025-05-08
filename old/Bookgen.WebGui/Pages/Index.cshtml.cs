
using BookGen.Interfaces;
using BookGen.WebGui.Services;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookGen.WebGui.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICurrentSession _currentSession;

        public IndexModel(ILogger<IndexModel> logger, ICurrentSession currentSession)
        {
            _logger = logger;
            _currentSession = currentSession;
            CurrentDirectory = _currentSession.StartDirectory;
            AppDirectory = _currentSession.AppDirectory;
            Version = _currentSession.Version;
        }

        public FsPath CurrentDirectory { get; }
        public FsPath AppDirectory { get; }
        public Version Version { get; }

        public void OnGet()
        {
        }
    }
}
