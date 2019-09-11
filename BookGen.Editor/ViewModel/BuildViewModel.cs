//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace BookGen.Editor.ViewModel
{
    internal class BuildViewModel
    {
        private const string EpubCommand = "BuildEpub";
        private const string WordpressCommand = "BuildWordpress";
        private const string PrintCommand = "BuildPrint";
        private const string TestCommand = "Test";
        private const string WebCommand = "BuildWeb";

        private const string CleanCommandstr = "Clean";
        private const string InitCommandstr = "Initialize";

        private readonly IExceptionHandler _exceptionHandler;

        public ICommand BuildEpupCommand { get; }
        public ICommand BuildTestWebsiteCommand { get; }
        public ICommand BuildWebsiteCommand { get; }
        public ICommand BuildWordpressCommand { get; }
        public ICommand BuildPrintCommand { get; }
        public ICommand CleanCommand { get; }
        public ICommand InitCommand { get; }

        private void RunBookGen(string command)
        {
            _exceptionHandler.SafeRun(() =>
            {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BookGen.exe");
                    process.StartInfo.Arguments = $"-d \"{EditorSessionManager.CurrentSession.DictionaryPath}\" -a {command}";
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                }
            });
        }

        public BuildViewModel(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
            BuildEpupCommand = new RelayCommand(OnBuildEpub);
            BuildTestWebsiteCommand = new RelayCommand(OnBuildTestWebsite);
            BuildWebsiteCommand = new RelayCommand(OnBuildWebsite);
            BuildWordpressCommand = new RelayCommand(BuildWordpress);
            BuildPrintCommand = new RelayCommand(BuildPrint);
            CleanCommand = new RelayCommand(OnClean);
            InitCommand = new RelayCommand(OnInit);
        }

        private void OnInit()
        {
            RunBookGen(InitCommandstr);
        }

        private void OnClean()
        {
            RunBookGen(CleanCommandstr);
        }

        private void BuildPrint()
        {
            RunBookGen(PrintCommand);
        }

        private void BuildWordpress()
        {
            RunBookGen(WordpressCommand);
        }

        private void OnBuildWebsite()
        {
            RunBookGen(WebCommand);
        }

        private void OnBuildTestWebsite()
        {
            RunBookGen(TestCommand);
        }

        private void OnBuildEpub()
        {
            RunBookGen(EpubCommand);
        }
    }
}
