using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight;

namespace BookGen.Editor.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        public FileBrowserViewModel FileExplorer { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IFileSystemServices fileSystemServices,
                             IExceptionHandler exceptionHandler)
        {
            FileExplorer = new FileBrowserViewModel(fileSystemServices, exceptionHandler);
            FileExplorer.RootDir = EditorSessionManager.CurrentSession.WorkDirectory;
        }
    }
}