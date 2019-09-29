//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;

namespace BookGen.Editor.Infrastructure.Jobs
{
    /// <summary>
    /// Interaction logic for JobWindow.xaml
    /// </summary>
    public partial class JobWindow : Window
    {
        private readonly CancellationTokenSource _tokensource;
        private readonly Progress<float> _progressReporter;
        private readonly bool _reportTaskBarProgress;

        private JobWindow()
        {
            InitializeComponent();
            _tokensource = new CancellationTokenSource();
            _progressReporter = new Progress<float>();
            _progressReporter.ProgressChanged += _progressReporter_ProgressChanged;
        }

        public JobWindow(string jobTitle, string jobDescription, bool reportTaskBarProgress) : this()
        {
            Title = jobTitle;
            Description.Text = jobDescription;
            _reportTaskBarProgress = reportTaskBarProgress;
        }

        public IProgress<float> Reporter
        {
            get { return _progressReporter; }
        }

        public CancellationToken CancelToken
        {
            get { return _tokensource.Token; }
        }

        private void _progressReporter_ProgressChanged(object sender, float e)
        {
            Dispatcher.Invoke(() =>
            {
                if (_reportTaskBarProgress)
                {
                    if (TaskBar.ProgressState != TaskbarItemProgressState.Normal)
                    {
                        TaskBar.ProgressState = TaskbarItemProgressState.Normal;
                    }
                    TaskBar.ProgressValue = (double)e;
                }
                ProgressBar.Value = e * 100.0d;
            });
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (_reportTaskBarProgress)
            {
                TaskBar.ProgressState = TaskbarItemProgressState.Paused;
            }
            _tokensource.Cancel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_reportTaskBarProgress)
            {
                TaskBar.ProgressState = TaskbarItemProgressState.Paused;
            }
            _tokensource.Cancel();
        }
    }
}
