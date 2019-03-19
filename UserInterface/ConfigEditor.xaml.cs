//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BookGen.UserInterface
{
    /// <summary>
    /// Interaction logic for ConfigEditor.xaml
    /// </summary>
    public partial class ConfigEditor : Window
    {
        private FsPath _config;
        private Config _cfg;

        public ConfigEditor(FsPath configFile)
        {
            InitializeComponent();
            _config = configFile;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var cfgstring = _config.ReadFile();
                _cfg = JsonConvert.DeserializeObject<Config>(cfgstring);
                PropertyEditor.SelectedObject = _cfg;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

        }

        private void SectionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SectionSelector.SelectedIndex)
            {
                case 0:
                    PropertyEditor.SelectedObject = _cfg;
                    break;
                case 1:
                    PropertyEditor.SelectedObject = _cfg.StyleClasses;
                    break;
                case 2:
                    PropertyEditor.SelectedObject = _cfg.SearchOptions;
                    break;
                case 3:
                    PropertyEditor.SelectedObject = _cfg.Metadata;
                    break;
                case 4:
                    PropertyEditor.SelectedObject = _cfg.PrecompileHeader;
                    break;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var modified = JsonConvert.SerializeObject(_config, Formatting.Indented);
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
