using System.Windows.Controls;

using BookGen.Launcher.ViewModels;

namespace BookGen.Launcher.Views;

public partial class MainPage : Page
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
