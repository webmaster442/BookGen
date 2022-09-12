using BookGen.Launcher.Properties;

namespace BookGen.Launcher.ViewModels
{
    internal class SettingsViewModel : ObservableObject
    {
        public bool UseWindowsTerminal
        {
            get => Settings.Default.UseWindowsTerminal;
            set
            {
                Settings.Default.UseWindowsTerminal = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(UseWindowsTerminal));
            }
        }

        public bool AutoExitLauncher
        {
            get => Settings.Default.AutoExitLauncher;
            set
            {
                Settings.Default.AutoExitLauncher = value;
                Settings.Default.Save();
                OnPropertyChanged(nameof(AutoExitLauncher));
            }
        }
    }
}
