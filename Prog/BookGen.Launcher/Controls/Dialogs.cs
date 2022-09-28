//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.Controls
{
    internal static class Dialog
    {
        public static bool TryselectFolderDialog(out string folderPath)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
            {
                Description = Properties.Resources.FolderselectDescription,
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false,
            };
            if (dialog.ShowDialog() == true)
            {
                folderPath = dialog.SelectedPath;
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
            dialog.ShowDialog();
            main.Blocker.Visibility = Visibility.Collapsed;

            return dialog.ClickedButton;
        }

    }
}
