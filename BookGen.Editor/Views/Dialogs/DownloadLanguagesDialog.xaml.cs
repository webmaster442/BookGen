//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using BookGen.Editor.Infrastructure.Jobs;
using BookGen.Editor.Models;
using BookGen.Editor.ServiceContracts;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BookGen.Editor.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for DownloadLanguagesDialog.xaml
    /// </summary>
    public partial class DownloadLanguagesDialog : Window
    {
        public BindableCollection<LanguageModel> NotInstalledItems { get; set; }
        public BindableCollection<LanguageModel> InstalledItems { get; set; }

        private readonly INHunspellServices _nHunspellServices;

        public DownloadLanguagesDialog(INHunspellServices nHunspellServices)
        {
            _nHunspellServices = nHunspellServices;
            NotInstalledItems = new BindableCollection<LanguageModel>();
            InstalledItems = new BindableCollection<LanguageModel>();
            RefreshLists();
            InitializeComponent();
            DataContext = this;
        }

        private void RefreshLists()
        {
            NotInstalledItems.Clear();
            InstalledItems.Clear();
            var models1 = CreateLanguageModels(_nHunspellServices.GetAvailableLanguages().Except(_nHunspellServices.GetInstalledLanguages()));
            var models2 = CreateLanguageModels(_nHunspellServices.GetInstalledLanguages());
            NotInstalledItems.AddRange(models1);
            InstalledItems.AddRange(models2);
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

        private async void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            var config = new JobRunnerConfiguration<IList<string>, bool>
            {
                JobTitle = "Installing Languages",
                JobDescription = "Downloading dictionaries...",
                ReportTaskBarProgress = true,
                JobInput = NotInstalledItems.Where(i => i.IsChecked).Select(i => i.Name).ToList(),
                JobFunction = (input, progress, ct) => _nHunspellServices.DownloadDictionaries(input, progress, ct)
            };

            await JobRunner.GetJobResultAsync(config).ConfigureAwait(false);
            RefreshLists();
        }

        private async void BtnUninstall_Click(object sender, RoutedEventArgs e)
        {
            var config = new JobRunnerConfiguration<IList<string>, bool>
            {
                JobTitle = "Uninstalling Languages",
                JobDescription = "Deleting dictionaries...",
                ReportTaskBarProgress = true,
                JobInput = InstalledItems.Where(i => i.IsChecked).Select(i => i.Name).ToList(),
                JobFunction = (input, progress, ct) => _nHunspellServices.DeleteDictionaries(input, progress, ct)
            };

            await JobRunner.GetJobResultAsync(config).ConfigureAwait(false);
            RefreshLists();
        }
    }
}
