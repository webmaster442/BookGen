using System;
using System.Threading;
using System.Windows;
using System.Windows.Shell;

namespace BookGen.Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IProgress<double>
    {
        private readonly CancellationTokenSource _tokenSource;

        public MainWindow()
        {
            InitializeComponent();
            _tokenSource = new CancellationTokenSource();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ContainerCancel.Visibility == Visibility.Visible
                && !CancelConfirm())
            {
                e.Cancel = true;
            }
            else
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
            }
        }

        private void BtnCancelClick_Click(object sender, RoutedEventArgs e)
        {
            if (CancelConfirm())
            {
                _tokenSource.Cancel();
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            TbFolderPath.IsEnabled = false;
            ContainerCopy.Visibility = Visibility.Collapsed;
            ContainerCancel.Visibility = Visibility.Visible;
        }

        private bool CancelConfirm()
        {
            var msg = MessageBox.Show("Cancel installation?", Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return msg == MessageBoxResult.Yes;
        }

        public void Report(double value)
        {
            Dispatcher.Invoke(() =>
            {
                InstallProgrss.Value = value;
                if (Math.Abs(value - 0) < 1E-3
                    || Math.Abs(value - 1) < 1E-3)
                {
                    TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                }
                else
                {
                    TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                }
                TaskbarItemInfo.ProgressValue = value;
            });
        }
    }
}