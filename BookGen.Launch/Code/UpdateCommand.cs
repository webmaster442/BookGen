//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Launch.Code
{
    public class UpdateCommand : ICommand
    {
#pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

        private readonly DateTime _version;
        private readonly string _appdir;

        public UpdateCommand()
        {
            var name = typeof(App).Assembly.GetName();
            _version = ConvertVersion(name?.Version);
            _appdir = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
        }

        private DateTime ConvertVersion(Version? version)
        {
            if (version == null)
                return DateTime.Now;
            else
                return new DateTime(version.Major, version.Minor, version.Build);
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }


        public void Execute(object? parameter)
        {
            if (Application.Current.MainWindow is not MainWindow main
                || string.IsNullOrEmpty(_appdir))
            {
                return;
            }
            var dialog = new UpdateDialog(_version, _appdir)
            {
                Owner = Application.Current.MainWindow,
                Width = Application.Current.MainWindow.ActualWidth * 0.6,
            };
            main.Blocker.Visibility = Visibility.Visible;
            dialog.ShowDialog();
            main.Blocker.Visibility = Visibility.Collapsed;
        }
    }
}