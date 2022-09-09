using System.Windows.Controls;

using BookGen.Launcher.ViewModels;

namespace BookGen.Launcher.Views;

public partial class FolderViewPage : Page
{
    public FolderViewPage(FolderViewViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
