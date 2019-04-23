//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.Dialogs;
using BookGen.Editor.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace BookGen.Editor.ViewModel
{
    internal class ActionsViewModel: ViewModelBase
    {
        private const string _configfile = "bookgen.json";
        private string _rootDir;
        private string _gitPath = @"%programfiles%\Git\git-bash.exe";

        public ICommand BuildCommand { get; }
        public ICommand OpenGitBashCommand { get; }
        public ICommand CreateConfigCommand { get; }
        public ICommand EditConfigCommand { get; }

        public string RootDir
        {
            get { return _rootDir; }
            set { SetValue(ref _rootDir, value); }
        }

        public ActionsViewModel()
        {
            BuildCommand = new DelegateCommand<string>(OnBuild, OnCanBuild);
            OpenGitBashCommand = new DelegateCommand<object>(OnGitBash, OnCanGitBash);
            CreateConfigCommand = DelegateCommand.CreateCommand(OnCreateConfig, OnCanCreateConfig);
            EditConfigCommand = DelegateCommand.CreateCommand(OnEditConfig, OnCanEditConfig);
            _gitPath = Environment.ExpandEnvironmentVariables(_gitPath);
        }

        private bool OnCanEditConfig(object obj)
        {
            var configFile = Path.Combine(RootDir, _configfile);
            return File.Exists(configFile);
        }

        private void OnEditConfig(object obj)
        {
            ConfigurationEditor editor = new ConfigurationEditor(new FsPath(RootDir).Combine(_configfile));
            editor.ShowDialog();
        }

        private void OnCreateConfig(object obj)
        {
            OnBuild(KnownArguments.CreateConfig);
        }

        private bool OnCanCreateConfig(object obj)
        {
            var configFile = Path.Combine(RootDir, _configfile);
            return !File.Exists(configFile);
        }

        private bool OnCanGitBash(object obj)
        {
            return File.Exists(_gitPath);
        }

        private void OnGitBash(object obj)
        {
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _gitPath,
                    WorkingDirectory = RootDir,
                }
            };
            p.Start();
        }

        private bool OnCanBuild(string obj)
        {
            var configFile = Path.Combine(RootDir, _configfile);
            return File.Exists(configFile);
        }

        private void OnBuild(string obj)
        {
            string args = $"-a {obj} -d \"{RootDir}\"";

            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BookGen.exe"),
                    WorkingDirectory = RootDir,
                    Arguments = args
                }
            };
            p.Start();
        }
    }
}
