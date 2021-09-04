//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Launch.Properties;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Launch
{

    internal interface IMainWindow
    {
        void ShowChangeLog();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
            Changelog.Text = Properties.Resources.Changelog;
        }

        private void WindowChrome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void WindowChromeMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void WindowChromeClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AppWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.Save();
        }

        private void CloseChangeLog(object sender, RoutedEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
        }

        public void ShowChangeLog()
        {
            Popup.Visibility = Visibility.Visible;
        }
    }
}
