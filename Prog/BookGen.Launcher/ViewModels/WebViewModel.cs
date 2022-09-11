namespace BookGen.Launcher.ViewModels
{
    internal class WebViewModel : ObservableObject
    {
        public string Content { get; }

        public WebViewModel(string? location)
        {
            if (string.IsNullOrEmpty(location))
            {
                Content = string.Empty;
                return;
            }

            string file = Path.Combine(AppContext.BaseDirectory, location);

            if (File.Exists(file))
            {
                Content = File.ReadAllText(file);
                return;
            }

            if (location.IsUrl())
            {
                Content = location;
                return;
            }

            Content = Properties.Resources.NotFound;

        }
    }
}
