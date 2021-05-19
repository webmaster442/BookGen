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
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(UseWindowsTerminal)));
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
            Launcher.Launcher.LaunchResult result;
            if (obj is string directory && 
                !string.IsNullOrEmpty(directory))
            {
                result = _launcher.Run(UseWindowsTerminal, directory);
                if (result == Launcher.Launcher.LaunchResult.FolderNoLongerExists)
                {
                    _registryAdapter.DeleteRecentItem(directory);
                    return;
                }
            }
            else
            {
                result = _launcher.Run(UseWindowsTerminal);
            }
            Application.Current.Shutdown((int)result);
        }

        private void OnClear(object obj)
        {
            if (MessageBox.Show(Properties.Resources.ClearRecentList,
                                Properties.Resources.Question,
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _registryAdapter.DeleteRecentDirectoryList();
            }
        }
    }
}
