//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using System.Windows;
using System.Windows.Controls;

namespace BookGen.Editor.Controls
{
    /// <summary>
    /// Interaction logic for SpellSelector.xaml
    /// </summary>
    internal partial class SpellSelector : UserControl
    {
        public SpellSelector()
        {
            InitializeComponent();
            SpellCheckDictionaries = new BindableCollection<string>();
            DataContext = this;
        }

        public MarkdownEditor Editor
        {
            get { return (MarkdownEditor)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        public static readonly DependencyProperty EditorProperty =
            DependencyProperty.Register("Editor", typeof(MarkdownEditor), typeof(SpellSelector), new PropertyMetadata(null, EditorConfigure));

        private static void EditorConfigure(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SpellSelector selector)
            {
                selector.RefreshList();
            }
        }

        private void RefreshList()
        {
            if (Editor.NHunspellServices != null)
            {
                SpellCheckDictionaries.Clear();
                SpellCheckDictionaries.AddRange(Editor.NHunspellServices.GetInstalledLanguages());
            }
        }

        public BindableCollection<string> SpellCheckDictionaries { get; set; }

        private void BtnGetLanguages_Click(object sender, RoutedEventArgs e)
        {
            if (Editor != null 
                && Editor.DialogService != null
                && Editor.NHunspellServices != null)
            {
                Editor.DialogService.ShowSpellSettingsConfiguration();
                RefreshList();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void LanguageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageSelection.SelectedIndex > -1)
            {
                string item = SpellCheckDictionaries[LanguageSelection.SelectedIndex];
                Properties.Settings.Default.Editor_SpellCheckLanguage = item;
                if (SpellEnabled.IsChecked == true
                    && Editor != null)
                {
                    Editor.ToggleSpeelCheck();
                }
            }
        }
    }
}
