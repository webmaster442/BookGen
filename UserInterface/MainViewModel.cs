//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace BookGen.UserInterface
{
    internal class MainViewModel : ViewModelBase
    {
        private string _workFolder;
        private const string _configfile = "bookgen.json";

        public string WorkingDirectory
        {
            get { return _workFolder; }
            set { SetValue(ref _workFolder, value); }
        }

        public ICommand OpenFolderCommand { get; }
        public ICommand CreateConfigCommand { get; }
        public ICommand EditConfigCommand { get; }
        public ICommand BuildCommand { get; }


        public MainViewModel()
        {
            OpenFolderCommand = DelegateCommand.CreateCommand(OnOpenFolder);
            CreateConfigCommand = DelegateCommand.CreateCommand(OnCreateConfig, OnCanCreateConfig);
            EditConfigCommand = DelegateCommand.CreateCommand(OnEditConfig, OnCanEditConfig);
            BuildCommand = new DelegateCommand<string>(OnBuild, OnCanBuild);
            _workFolder = Environment.CurrentDirectory;
        }

        private void OnOpenFolder(object obj)
        {
            using (var fb = new System.Windows.Forms.FolderBrowserDialog())
            {
                fb.SelectedPath = _workFolder;
                if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    WorkingDirectory = fb.SelectedPath;
                }
            }
        }

        private bool OnCanEditConfig(object obj)
        {
            var configFile = Path.Combine(WorkingDirectory, _configfile);
            return File.Exists(configFile);
        }

        private void OnEditConfig(object obj)
        {
            ConfigEditor editor = new ConfigEditor(new FsPath(WorkingDirectory).Combine(_configfile));
            editor.ShowDialog();
        }

        private void OnCreateConfig(object obj)
        {
            OnBuild("createconfig");
        }

        private bool OnCanCreateConfig(object obj)
        {
            var configFile = Path.Combine(WorkingDirectory, _configfile);
            return !File.Exists(configFile);
        }

        private bool OnCanBuild(string obj)
        {
            var configFile = Path.Combine(WorkingDirectory, _configfile);
            return File.Exists(configFile);
        }

        private void OnBuild(string arguments)
        {
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BookGen.exe"),
                    WorkingDirectory = WorkingDirectory,
                    Arguments = arguments
                }
            };
            p.Start();
        }
    }
}
