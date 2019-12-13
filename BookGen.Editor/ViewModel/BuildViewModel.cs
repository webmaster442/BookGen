//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight.Command;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace BookGen.Editor.ViewModel
{
    internal class BuildViewModel
    {
        private const string CleanCommandstr = "Clean";
        private const string InitCommandstr = "Initialize";

        private readonly IExceptionHandler _exceptionHandler;
        private readonly IDialogService _dialogService;

        public ICommand CleanCommand { get; }
        public ICommand InitCommand { get; }
        public ICommand RunBookGenCommand { get; }

        public ICommand OpenFileExplorerCommand { get; }

        private void RunBookGen(bool gui, string command)
        {
            _exceptionHandler.SafeRun(() =>
            {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BookGen.exe");
                    if (gui)
                    {
                        process.StartInfo.Arguments = $"-d \"{EditorSessionManager.CurrentSession.DictionaryPath}\" -g";
                    }
                    else
                    {
                        process.StartInfo.Arguments = $"-d \"{EditorSessionManager.CurrentSession.DictionaryPath}\" -a {command}";
                    }
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                }
            });
        }

        public BuildViewModel(IExceptionHandler exceptionHandler,
                              IDialogService dialogService)
        {
            _dialogService = dialogService; 
            _exceptionHandler = exceptionHandler;
            CleanCommand = new RelayCommand(OnClean);
            InitCommand = new RelayCommand(OnInit);
            RunBookGenCommand = new RelayCommand(OnRunBookGen);
            OpenFileExplorerCommand = new RelayCommand(OnOpenFileExplorer);
        }

        private void OnRunBookGen()
        {
            RunBookGen(true, string.Empty);
        }

        private void OnOpenFileExplorer()
        {
            _dialogService.OpenFileExplorer();
        }

        private void OnInit()
        {
            RunBookGen(false, InitCommandstr);
        }

        private void OnClean()
        {
            RunBookGen(false, CleanCommandstr);
        }
    }
}
