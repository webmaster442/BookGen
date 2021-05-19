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
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(UseWindowsTerminal)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _launcher = new();
            _registryAdapter = new(key);
            DataContext = this;
            RecentFiles = new ObservableCollection<string>(new[]
{
                    "c:\\test",
                    "d:\\test\\book",
                    "e:\\aafw\\wfwfg\\ee",
                    "e:\\aafw\\wfwfg\\ee",
                    "e:\\aafw\\wfwfg\\ee",
                    "e:\\aafw\\wfwfg\\ee",
                });

            //RecentFiles = new(_registryAdapter.GetRecentDirectoryList());
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
            throw new NotImplementedException();
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
