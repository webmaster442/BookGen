//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows;

namespace BookGen.Editor.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfigurationEditor.xaml
    /// </summary>
    public partial class ConfigurationEditor : Window, INotifyPropertyChanged
    {
        private FsPath _config;
        private Config _configuration;

        public event PropertyChangedEventHandler PropertyChanged;

        public Config Config
        {
            get { return _configuration; }
            set
            {
                _configuration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Config)));
            }
        }

        public ConfigurationEditor(FsPath configFile)
        {
            InitializeComponent();
            _config = configFile;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var cfgstring = _config.ReadFile();
                Config = JsonConvert.DeserializeObject<Config>(cfgstring);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var modified = JsonConvert.SerializeObject(Config, Formatting.Indented);
                _config.WriteFile(modified);
                MessageBox.Show("Settings saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
