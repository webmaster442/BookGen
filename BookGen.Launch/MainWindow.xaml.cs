using BookGen.Launch.Launcher;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace BookGen.Launch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string key = "BookGen.Launcher";
        private readonly RegistryAdapter _registryAdapter;
        private readonly Launcher.Launcher _launcher;

        private ObservableCollection<string> RecentFiles { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _launcher = new();
            _registryAdapter = new(key);
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
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _registryAdapter.SaveRecentDirectoryList(RecentFiles);
        }
    }
}
