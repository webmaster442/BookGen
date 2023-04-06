//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels
{
    internal sealed class WebViewModel : ObservableObject
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

            if (location.IsUrl() || location.StartsWith("<!doctype html>", StringComparison.InvariantCultureIgnoreCase))
            {
                Content = location;
                return;
            }

            Content = Properties.Resources.NotFound;

        }
    }
}
