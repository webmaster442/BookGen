using System.Windows.Controls;

using BookGen.Launcher.Contracts.Views;
using BookGen.Launcher.ViewModels;

using MahApps.Metro.Controls;

namespace BookGen.Launcher.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame()
        => shellFrame;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
