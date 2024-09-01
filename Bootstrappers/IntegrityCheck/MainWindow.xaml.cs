using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;

using Bookgen.Win;

namespace IntegrityCheck
{
    public partial class MainWindow : Window, IResultProgrss
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly string _integrityFile;
        private readonly string _dataFolder;
        private bool _isRunning;

        public MainWindow()
        {
            InitializeComponent();
            _cancellationTokenSource = new CancellationTokenSource();
            _integrityFile = Path.Combine(AppContext.BaseDirectory, "integrity.xml");
            _dataFolder = Path.Combine(AppContext.BaseDirectory, "data");
            if (!Directory.Exists(_dataFolder))
            {
                _dataFolder = AppContext.BaseDirectory;
            }
        }

        private void Exit(bool calledByCancel)
        {
            if (_isRunning)
            {
                _cancellationTokenSource.Cancel();
            }
            _cancellationTokenSource.Dispose();
            if (calledByCancel)
            {
                Close();
            }
        }
        private void WriteXml(List<IntegrityItem> results)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<IntegrityItem>));
            using (var stream = File.Create(_integrityFile))
            {
                xs.Serialize(stream, results);
            }
        }

        private List<IntegrityItem> ReadXml()
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<IntegrityItem>));
            using (var stream = File.OpenRead(_integrityFile))
            {
                return (List<IntegrityItem>)xs.Deserialize(stream);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => Exit(calledByCancel: true);

        private void Window_Closing(object sender, CancelEventArgs e)
            => Exit(calledByCancel: false);

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            _isRunning = true;
            if (args.Length == 2
                && args[1] == "/compute")
            {
                var results = await Bookgen.Win.IntegrityCheck.Create(_dataFolder, this, _cancellationTokenSource.Token);
                WriteXml(results);
            }
            else if (File.Exists(_integrityFile))
            {
                var items = ReadXml();
                _isRunning = await Bookgen.Win.IntegrityCheck.Verify(_dataFolder, items, this, _cancellationTokenSource.Token);
            }
            else
            {
                MessageBox.Show("Integrity file not found. Please resinstall program.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            _isRunning = false;
            BtnCancel.Content = "Exit";
        }

        void IResultProgrss.ReportFailed(string fileName)
            => LbFailed.Items.Add(fileName);

        void ICountProgress.SetMaximum(int max)
        {
            PbProgress.Minimum = 0;
            PbProgress.Maximum = max;
        }

        void IProgress<int>.Report(int value)
            => PbProgress.Value = value;
    }
}