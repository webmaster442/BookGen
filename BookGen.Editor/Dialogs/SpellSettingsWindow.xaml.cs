//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;

namespace BookGen.Editor.Dialogs
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SpellSettingsWindow : Window
    {
        public SpellSettingsWindow()
        {
            InitializeComponent();
        }

        private string SelectFile(string filter)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = filter;
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }
            return null;
        }

        private void BtnBrowseDict_Click(object sender, RoutedEventArgs e)
        {
            var selected = SelectFile("Hunspell *.dic files| *.dic");
            if (!string.IsNullOrEmpty(selected))
            {
                Properties.Settings.Default.Editor_DictFile = selected;
            }
        }

        private void BtnBrowseAff_Click(object sender, RoutedEventArgs e)
        {
            var selected = SelectFile("Hunspell *.aff files| *.aff");
            if (!string.IsNullOrEmpty(selected))
            {
                Properties.Settings.Default.Editor_AffFile = selected;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}