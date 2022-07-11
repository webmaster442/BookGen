//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using BookGen.DomainServices;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BookGen.Launch
{
    /// <summary>
    /// Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : Window, ILog
    {
        #region ILog implementation
        public LogLevel LogLevel { get; set; }

        public event EventHandler<LogEventArgs>? OnLogWritten;

        public void Log(LogLevel logLevel, string format, params object[] args)
        {
            Dispatcher.Invoke(() =>
            {
                string text = string.Format(format, args);
                string line = string.Format("{0} | {1} | {2}", DateTime.Now.ToShortTimeString(), logLevel, text);
                LogEntries.Add(line);
                OnLogWritten?.Invoke(this, new LogEventArgs(logLevel, line));
            });
        }

        public void PrintLine(string str)
        {
            Dispatcher.Invoke(() => LogEntries.Add(str));
        }

        public void PrintLine(object obj)
        {
            Dispatcher.Invoke(() => LogEntries.Add(obj.ToString() ?? ""));
        }
        #endregion

        public ObservableCollection<string> LogEntries { get; }
        private readonly CancellationTokenSource _tokenSource;
        private readonly Updater _updater;
        private bool _searching;
        private Release? _latest;

        public UpdateDialog(DateTime version, string appdir)
        {
            InitializeComponent();
            _tokenSource = new CancellationTokenSource();
            LogEntries = new ObservableCollection<string>();
            LogBox.ItemsSource = LogEntries;
            BtnCancel.IsEnabled = true;
            _updater = new Updater(this, version, appdir);
        }

        private void UpdateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(CheckForUptates);
        }

        private async Task CheckForUptates()
        {
            Log(LogLevel.Info, "Checking for updates...");
            _searching = true;
            Dispatcher.Invoke(() => BtnOk.Visibility = Visibility.Collapsed);
            _latest = await _updater.GetLatestReleaseAsync(_tokenSource.Token, false);
            if (_latest != null &&
                _updater.IsUpdateNewerThanCurrentVersion(_latest))
            {
                Log(LogLevel.Info, "Update found. Click Ok to start update");
            }
            else
            {
                Log(LogLevel.Info, "Update not found");
                Dispatcher.Invoke(() => BtnCancel.Visibility = Visibility.Collapsed);
            }
            Dispatcher.Invoke(() => BtnOk.Visibility = Visibility.Visible);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (_latest != null
                && _updater.IsUpdateNewerThanCurrentVersion(_latest))
            {
                _updater.LaunchUpdateScript(_latest);
            }
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_searching)
            {
                _tokenSource.Cancel();
            }
            DialogResult = false;
        }
    }
}
