using BookGen.Launcher.Interfaces;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;

namespace BookGen.Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IMainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }

        public void ClosePopup()
        {
            ContentPopup.Content = null;
            ContentPopup.Visibility = Visibility.Collapsed;
        }

        public void LoadIntoMain(INotifyPropertyChanged viewModel)
        {
            ContentPopup.Content = null;
            ContentPopup.Visibility = Visibility.Collapsed;
            ContentMain.Content = viewModel;
            ContentMain.Visibility = Visibility.Visible;
        }

        public void LoadIntoPopup(INotifyPropertyChanged viewModel)
        {
            ContentPopup.Content = viewModel;
            ContentPopup.Visibility = Visibility.Visible;
        }
    }
}
