using System.Windows.Controls;

namespace BookGen.Launcher.Contracts.Views;

public interface IShellWindow
{
    Frame GetNavigationFrame();

    void ShowWindow();

    void CloseWindow();
}
