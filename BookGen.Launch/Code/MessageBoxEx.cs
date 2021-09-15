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
                Width = Application.Current.MainWindow.ActualWidth * 0.8,
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
    }
}
