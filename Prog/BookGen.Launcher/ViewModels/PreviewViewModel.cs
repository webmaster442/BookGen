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
