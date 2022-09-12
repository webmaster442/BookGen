//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels
{
    internal class PreviewViewModel : ObservableObject
    {
        public PreviewViewModel(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }
}
