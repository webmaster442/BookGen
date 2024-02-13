//-----------------------------------------------------------------------------
// (c) 2021-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels;

internal sealed class SettingsViewModel : ObservableObject
{
    public bool UseWindowsTerminal
    {
        get => Properties.Settings.Default.UseWindowsTerminal;
        set
        {
            Properties.Settings.Default.UseWindowsTerminal = value;
            Properties.Settings.Default.Save();
            OnPropertyChanged(nameof(UseWindowsTerminal));
        }
    }

    public bool AutoExitLauncher
    {
        get => Properties.Settings.Default.AutoExitLauncher;
        set
        {
            Properties.Settings.Default.AutoExitLauncher = value;
            Properties.Settings.Default.Save();
            OnPropertyChanged(nameof(AutoExitLauncher));
        }
    }
}
