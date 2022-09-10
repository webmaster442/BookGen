using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Launcher.ViewModels
{
    internal class FileBrowserItemViewModel
    {
        public string FullPath { get; init; }
        public bool IsFolder { get; init; }
        public DateTime ModificationDate { get; init; }
        public long Size { get; init; }
        public string Extension { get; init; }

        public FileBrowserItemViewModel()
        {
            FullPath = string.Empty;
            Extension = string.Empty;
        }
    }
}
