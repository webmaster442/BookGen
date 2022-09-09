using System.Windows.Controls;

using BookGen.Launcher.ViewModels;

namespace BookGen.Launcher.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
