using BookGen.Launch.Launcher;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace BookGen.Launch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string key = "BookGen.Launcher";
        private readonly RegistryAdapter _registryAdapter;
        private readonly Launcher.Launcher _launcher;
        private bool useWindowsTerminal;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<string> RecentFiles { get; set; }
        public DelegateCommand OpenCommand { get; }
        public DelegateCommand ClearCommand { get; }

        public bool UseWindowsTerminal
        {
            get => useWindowsTerminal;
            set
            {
                useWindowsTerminal = value;
                _registryAdapter.SaveWindowsTerminal(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseWindowsTerminal)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _launcher = new();
            _registryAdapter = new(key);
            UseWindowsTerminal = _registryAdapter.GetWindowsTerminalUsage() ?? true;
            DataContext = this;
            RecentFiles = new(_registryAdapter.GetRecentDirectoryList());
            PART_Items.ItemsSource = RecentFiles;
            OpenCommand = new DelegateCommand(OnOpen);
            ClearCommand = new DelegateCommand(OnClear);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _registryAdapter.SaveRecentDirectoryList(RecentFiles);
        }

        private void OnOpen(object obj)
        {
            (Launcher.Launcher.LaunchResult result, string selectedFolder) launcherResult;
            if (obj is string directory && 
                !string.IsNullOrEmpty(directory))
            {
                launcherResult = _launcher.Run(UseWindowsTerminal, directory);
                if (launcherResult.result == Launcher.Launcher.LaunchResult.FolderNoLongerExists)
                {
                    _registryAdapter.DeleteRecentItem(directory);
                    return;
                }
            }
            else
            {
                launcherResult = _launcher.Run(UseWindowsTerminal);
            }

            if (launcherResult.result == Launcher.Launcher.LaunchResult.Ok)
            {
                if (RecentFiles.Contains(launcherResult.selectedFolder))
                {
                    RecentFiles.Remove(launcherResult.selectedFolder);
                }
                RecentFiles.Insert(0, launcherResult.selectedFolder);
                Application.Current.Shutdown((int)launcherResult.result);
            }
        }

        private void OnClear(object obj)
        {
            if (MessageBox.Show(Properties.Resources.ClearRecentList,
                                Properties.Resources.Question,
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RecentFiles.Clear();
                _registryAdapter.DeleteRecentDirectoryList();
            }
        }
    }
}
