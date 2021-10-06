//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain;
using BookGen.Framework.Scripts;
using BookGen.Framework.Server;
using BookGen.GeneratorSteps;
using BookGen.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using Webmaster442.HttpServerFramework;

namespace BookGen
{
    internal class GeneratorRunner
    {
        private readonly CsharpScriptHandler _scriptHandler;
        private readonly ProjectLoader _projectLoader;

        public const string ExitString = "Press a key to exit...";

        private Config? _configuration;
        private ToC? _toc;

        public FsPath ConfigFile { get; private set; }

        public string WorkDirectory
        {
            get;
        }

        public ILog Log { get; }
        public IServerLog ServerLog { get; }
        public bool NoWait { get; internal set; }

        public GeneratorRunner(ILog log, IServerLog serverLog, string workDir)
        {
            ServerLog = serverLog;
            Log = log;
            _projectLoader = new ProjectLoader(log, workDir);
            _scriptHandler = new CsharpScriptHandler(Log);
            WorkDirectory = workDir;
            ConfigFile = new FsPath(WorkDirectory, "bookgen.json");
            _configuration = new Config();
            _toc = new ToC();
        }

        public void RunHelp()
        {
            Console.WriteLine(HelpUtils.GetGeneralHelp());
        }

        #region Helpers

        public void InitializeAndExecute(Action<GeneratorRunner> actionToExecute)
        {
            if (Initialize())
            {
                actionToExecute.Invoke(this);
            }
            else
            {
                Program.Exit(ExitCode.BadConfiguration);
            }
        }

        public bool Initialize(bool compileScripts = true)
        {
            Log.Info("---------------------------------------------------------");
            Log.Info("BookGen Build date: {0:yyyy.MM.dd} Starting...", Program.CurrentState.BuildDate.Date);
            Log.Info("Config API version: {0}", Program.CurrentState.ProgramVersion);
            Log.Info("Working directory: {0}", WorkDirectory);
            Log.Info("Os: {0}", Environment.OSVersion.VersionString);
            Log.Info("---------------------------------------------------------");


            bool ret = _projectLoader.TryLoadAndValidateConfig(out _configuration)
                && _projectLoader.TryLoadAndValidateToc(_configuration, out _toc);

            if (compileScripts)
                ret = ret && LoadAndCompileScripts();

            if (!ret && !NoWait)
                Program.ShowMessageBox(ExitString);

            return ret;
        }

        private bool LoadAndCompileScripts()
        {
            if (_configuration == null) return false;

            if (string.IsNullOrEmpty(_configuration.ScriptsDirectory)) return true;

            Log.Info("Trying to load and compile script files...");
            FsPath scripts = new FsPath(WorkDirectory).Combine(_configuration.ScriptsDirectory);

            int count = _scriptHandler.LoadScripts(scripts);
            Log.Info("Loaded {0} instances from script files", count);

            return true;
        }

        public void DoClean()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is null");

            CreateOutputDirectory.CleanDirectory(new FsPath(_configuration.TargetWeb.OutPutDirectory), Log);
            CreateOutputDirectory.CleanDirectory(new FsPath(_configuration.TargetPrint.OutPutDirectory), Log);
            CreateOutputDirectory.CleanDirectory(new FsPath(_configuration.TargetEpub.OutPutDirectory), Log);
            CreateOutputDirectory.CleanDirectory(new FsPath(_configuration.TargetWordpress.OutPutDirectory), Log);
        }
        #endregion

        #region Argument handlers

        public void DoBuild()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is null");

            if (_toc == null)
                throw new InvalidOperationException("Table of contents is null");

            var settings = _projectLoader.CreateRuntimeSettings(_configuration, _toc, _configuration.TargetWeb);

            Log.Info("Building deploy configuration...");

            using (var loader = new ShortCodeLoader(Log, settings, Program.AppSetting))
            {
                WebsiteBuilder builder = new WebsiteBuilder(settings, Log, loader, _scriptHandler);
                var runTime = builder.Run();
                Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
            }
        }

        public void DoPrint()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is null");

            if (_toc == null)
                throw new InvalidOperationException("Table of contents is null");

            var settings = _projectLoader.CreateRuntimeSettings(_configuration, _toc, _configuration.TargetPrint);

            Log.Info("Building print configuration...");

            using (var loader = new ShortCodeLoader(Log, settings, Program.AppSetting))
            {
                PrintBuilder builder = new PrintBuilder(settings, Log, loader, _scriptHandler);
                var runTime = builder.Run();
                Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
            }
        }

        public void DoEpub()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is null");

            if (_toc == null)
                throw new InvalidOperationException("Table of contents is null");

            var settings = _projectLoader.CreateRuntimeSettings(_configuration, _toc, _configuration.TargetEpub);

            Log.Info("Building epub configuration...");

            using (var loader = new ShortCodeLoader(Log, settings, Program.AppSetting))
            {
                EpubBuilder builder = new EpubBuilder(settings, Log, loader, _scriptHandler);
                var runTime = builder.Run();
                Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
            }
        }

        public void DoWordpress()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is null");

            if (_toc == null)
                throw new InvalidOperationException("Table of contents is null");

            var settings = _projectLoader.CreateRuntimeSettings(_configuration, _toc, _configuration.TargetWordpress);

            Log.Info("Building Wordpress configuration...");

            using (var loader = new ShortCodeLoader(Log, settings, Program.AppSetting))
            {
                WordpressBuilder builder = new WordpressBuilder(settings, Log, loader, _scriptHandler);
                var runTime = builder.Run();
                Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
            }
        }

        public void DoTest()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is null");

            if (_toc == null)
                throw new InvalidOperationException("Table of contents is null");

            Log.Info("Building test configuration...");
            _configuration.HostName = "http://localhost:8080/";

            var settings = _projectLoader.CreateRuntimeSettings(_configuration, _toc, _configuration.TargetWeb);


            using (var loader = new ShortCodeLoader(Log, settings, Program.AppSetting))
            {
                WebsiteBuilder builder = new WebsiteBuilder(settings, Log, loader, _scriptHandler);
                var runTime = builder.Run();

                using (var server = HttpServerFactory.CreateServerForTest(ServerLog, Path.Combine(WorkDirectory, _configuration.TargetWeb.OutPutDirectory)))
                {
                    server.Start();
                    Log.Info("-------------------------------------------------");
                    Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
                    Log.Info("Test server running on: http://localhost:8080/");
                    Log.Info("Serving from: {0}", _configuration.TargetWeb.OutPutDirectory);

                    if (Program.AppSetting.AutoStartWebserver)
                    {
                        UrlOpener.OpenUrl(_configuration.HostName);
                    }

                    Console.WriteLine(ExitString);
                    Console.ReadLine();
                    server.Stop();
                }
            }
        }

        #endregion
    }
}
