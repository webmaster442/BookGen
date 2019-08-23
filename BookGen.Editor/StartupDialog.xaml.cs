//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Models;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BookGen.Editor
{
    /// <summary>
    /// Interaction logic for StartupDialog.xaml
    /// </summary>
    public partial class StartupDialog : MetroWindow
    {
        private readonly EditorSession _session;

        public ObservableCollection<string> Folders { get; set; }

        public StartupDialog(EditorSession session)
        {
            InitializeComponent();
            _session = session;

            if (session.PreviousWorkdirs == null)
                Folders = new ObservableCollection<string>();
            else
                Folders = new ObservableCollection<string>(session.PreviousWorkdirs);

            WorkDirs.ItemsSource = Folders;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select working directory";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _session.WorkDirectory = dialog.SelectedPath;
                    _session.PreviousWorkdirs.Add(dialog.SelectedPath);
                    DialogResult = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                var path = btn.CommandParameter as string;
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    _session.WorkDirectory = path;
                    DialogResult = true;
                }
                else
                {
                    var remove = MessageBox.Show("Directory doesn't exist. Remove it from list?", "Warning", 
                                                 MessageBoxButton.YesNoCancel, 
                                                 MessageBoxImage.Question);

                    if (remove == MessageBoxResult.Yes)
                    {
                        _session.PreviousWorkdirs.Remove(path);
                        Folders.Remove(path);
                        WorkDirs.ItemsSource = Folders;
                    }
                }
            }
        }
    }
}
