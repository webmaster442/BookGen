//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Win32;

namespace BookGen.Launcher.Controls;

internal static class Dialog
{
    public static bool TryselectFolderDialog(out string folderPath)
    {
        var dialog = new OpenFolderDialog
        {
            Title = Properties.Resources.FolderselectDescription
        };
        if (dialog.ShowDialog() == true)
        {
            folderPath = dialog.FolderName;
            return true;
        }
        else
        {
            folderPath = string.Empty;
            return false;
        }
    }

    public static MessageBoxResult ShowMessageBox(string content, string title, MessageBoxButton buttons, MessageBoxImage image)
    {
        if (Application.Current.MainWindow is not MainWindow main)
        {
            return MessageBoxResult.None;
        }
        var dialog = new MessageDialog
        {
            Owner = Application.Current.MainWindow,
            Width = Application.Current.MainWindow.ActualWidth * 0.6,
            Title = title,
            DialogText = content,
            Image = image,
            Buttons = buttons
        };

        if (main.WindowState == WindowState.Minimized)
            main.WindowState = WindowState.Normal;

        main.Blocker.Visibility = Visibility.Visible;
        if (dialog.ShowDialog() != null)
        {
            main.Blocker.Visibility = Visibility.Collapsed;
        }

        return dialog.ClickedButton;
    }

}
