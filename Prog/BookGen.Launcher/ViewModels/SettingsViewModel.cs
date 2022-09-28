//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Launcher.Properties;

namespace BookGen.Launcher.ViewModels
{
    internal sealed class SettingsViewModel : ObservableObject
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
