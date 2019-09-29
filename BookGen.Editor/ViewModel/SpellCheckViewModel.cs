//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace BookGen.Editor.ViewModel
{
    internal class SpellCheckViewModel: ViewModelBase
    {
        private readonly INHunspellServices _nHunspellServices;
        private readonly IDialogService _dialogService;

        public SpellCheckViewModel(INHunspellServices hunspellServices, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _nHunspellServices = hunspellServices;
            SpellCheckDictionaries = new BindableCollection<string>(_nHunspellServices.GetAvailableLanguages());
            OpenLanguageDownloadsCommand = new RelayCommand(OnOpenLanguageDownload);
        }

        private void OnOpenLanguageDownload()
        {
            _dialogService.ShowSpellSettingsConfiguration();
            SpellCheckDictionaries.Clear();
            SpellCheckDictionaries.AddRange(_nHunspellServices.GetAvailableLanguages());
        }

        public BindableCollection<string> SpellCheckDictionaries { get; set; }

        public ICommand OpenLanguageDownloadsCommand { get; set; }
    }
}
