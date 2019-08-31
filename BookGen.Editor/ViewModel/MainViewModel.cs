//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.EditorControl;
using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight;

namespace BookGen.Editor.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private IMarkdownEditor _editor;

        public FileBrowserViewModel FileExplorer { get; }

        public IMarkdownEditor Editor
        {
            get { return _editor; }
            set
            {
                if (_editor != value)
                {
                    _editor = value;
                    RaisePropertyChanged(nameof(Editor));
                }
            }
        }

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