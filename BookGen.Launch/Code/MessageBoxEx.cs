//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;

namespace BookGen.Launch.Code
{
    internal static class MessageBoxEx
    {
        public static MessageBoxResult Show(string content, string title, MessageBoxButton buttons, MessageBoxImage image)
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
            main.Blocker.Visibility = Visibility.Visible;
            dialog.ShowDialog();
            main.Blocker.Visibility = Visibility.Collapsed;
            return dialog.ClickedButton;
        }

        public static void ShowDocument(string title, string fileName)
        {
            if (Application.Current.MainWindow is not MainWindow main)
            {
                return;
            }

            string text = "File can't be displayed";
            if (System.IO.File.Exists(fileName))
            {
                text = System.IO.File.ReadAllText(fileName);
            }
            var dialog = new DocumentDialog(main, title, text);
            dialog.Owner = Application.Current.MainWindow;
            main.Blocker.Visibility = Visibility.Visible;
            dialog.ShowDialog();
            main.Blocker.Visibility = Visibility.Collapsed;
        }
    }
}
