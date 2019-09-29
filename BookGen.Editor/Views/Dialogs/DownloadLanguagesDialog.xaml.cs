//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using BookGen.Editor.Models;
using BookGen.Editor.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace BookGen.Editor.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for DownloadLanguagesDialog.xaml
    /// </summary>
    public partial class DownloadLanguagesDialog : Window
    {
        public BindableCollection<LanguageModel> NotInstalledItems { get; set; }
        public BindableCollection<LanguageModel> InstalledItems { get; set; }

        public DownloadLanguagesDialog(INHunspellServices _nHunspellServices)
        {
            NotInstalledItems = new BindableCollection<LanguageModel>(CreateLanguageModels(_nHunspellServices.GetAvailableLanguages().Except(_nHunspellServices.GetInstalledLanguages())));
            InstalledItems = new BindableCollection<LanguageModel>(CreateLanguageModels(_nHunspellServices.GetInstalledLanguages()));
            InitializeComponent();
        }

        private IEnumerable<LanguageModel> CreateLanguageModels(IEnumerable<string> source)
        {
            foreach (var item in source)
            {
                yield return new LanguageModel
                {
                    IsChecked = false,
                    Name = item
                };
            }
        }

        private void BtnInstall_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUninstall_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
