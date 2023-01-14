//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Framework;
using BookGen.Framework.Scripts;
using BookGen.Framework.Server;
using BookGen.GeneratorStepRunners;
using BookGen.GeneratorSteps;
using BookGen.Interfaces;
using BookGen.ProjectHandling;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Webmaster442.HttpServerFramework;

namespace BookGen
{
    internal class GeneratorRunner
    {
        private readonly CsharpScriptHandler _scriptHandler;
        private readonly ProjectLoader _projectLoader;

        public const string ExitString = "Press a key to exit...";

        private readonly Config? _configuration;
        private readonly ToC? _toc;
        private readonly TagUtils? _tags;

        public FsPath ConfigFile { get; private set; }

        public string WorkDirectory
        {
            get;
        }

        public ILog Log { get; }
        public IServerLog ServerLog { get; }
        public bool NoWait { get; internal set; }

        public bool IsBookGenFolder => _projectLoader.IsBookGenFolder;

        public GeneratorRunner(ILog log, IServerLog serverLog, string workDir)
        {
            ServerLog = serverLog;
            Log = log;
            _projectLoader = new ProjectLoader(workDir, log);
            _scriptHandler = new CsharpScriptHandler(Log);
            WorkDirectory = workDir;
            ConfigFile = new FsPath(WorkDirectory, "bookgen.json");
            _configuration = new Config();
            _toc = new ToC();
            _tags = new TagUtils();
        }

        public void RunHelp()
        {
            Console.WriteLine(HelpUtils.GetGeneralHelp());
        }

        #region Helpers

        [MemberNotNull(nameof(_configuration), nameof(_toc), nameof(_tags))]
        private void ThrowIfInvalidState()
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is null");

            if (_toc == null)
                throw new InvalidOperationException("Table of contents is null");

            if (_tags == null)
                throw new InvalidOperationException("Tags is null");

        }

        public bool InitializeAndExecute(Action<GeneratorRunner> actionToExecute)
        {
            if (Initialize())
            {
                actionToExecute.Invoke(this);
                return true;
            }
            else
            {
                Program.Exit(ExitCode.BadConfiguration);
                return false;
            }
        }

        public bool Initialize(bool compileScripts = true)
        {
            Log.Info("---------------------------------------------------------");
            Log.Info("BookGen Build date: {0:yyyy.MM.dd} Starting...", Program.CurrentState.BuildDateUtc.Date);
            Log.Info("Config API version: {0}", Program.CurrentState.ProgramVersion);
            Log.Info("Working directory: {0}", WorkDirectory);
            Log.Info("Os: {0}", Environment.OSVersion.VersionString);
            Log.Info("---------------------------------------------------------");


            bool ret = _projectLoader.LoadProject();

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

        private void RunSteps<TBuilder>(Func<ShortCodeLoader, TBuilder> builderCreator, RuntimeSettings settings) where TBuilder : BookGen.Framework.GeneratorStepRunner
        {
            using (var loader = new ShortCodeLoader(Log, settings, Program.AppSetting))
            {
                using (TBuilder instance = builderCreator(loader))
                {
                    TimeSpan runTime = instance.Run();
                    Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
                }
            }
        }

        public void DoBuild()
        {
            ThrowIfInvalidState();

            RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_configuration.TargetWeb);

            Log.Info("Building deploy configuration...");

            RunSteps((loader) => new WebsiteGeneratorStepRunner(settings, Log, loader, _scriptHandler), settings);
        }

        public void DoPrint()
        {
            ThrowIfInvalidState();

            RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_configuration.TargetPrint);

            Log.Info("Building print configuration...");

            RunSteps((loader) => new PrintGeneratorStepRunner(settings, Log, loader, _scriptHandler), settings);
        }

        public void DoEpub()
        {
            ThrowIfInvalidState();

            RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_configuration.TargetEpub);

            Log.Info("Building epub configuration...");

            RunSteps((loader) => new EpubGeneratorStepRunner(settings, Log, loader, _scriptHandler), settings);
        }

        public void DoWordpress()
        {
            ThrowIfInvalidState();

            RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_configuration.TargetWordpress);

            Log.Info("Building Wordpress configuration...");

            RunSteps((loader) => new WordpressGeneratorStepRunner(settings, Log, loader, _scriptHandler), settings);
        }

        public void DoPostProcess()
        {
            ThrowIfInvalidState();

            RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_configuration.TargetPostProcess);

            Log.Info("Building postprocess configuration...");

            RunSteps((loader) => new PostProcessGenreratorStepRunner(settings, Log, loader, _scriptHandler), settings);
        }

        public void DoTest()
        {
            ThrowIfInvalidState();

            Log.Info("Building test configuration...");

            _configuration.HostName = "http://localhost:8090/";

            RuntimeSettings? settings = _projectLoader.CreateRuntimeSettings(_configuration.TargetWeb);


            using (var loader = new ShortCodeLoader(Log, settings, Program.AppSetting))
            {
                var builder = new WebsiteGeneratorStepRunner(settings, Log, loader, _scriptHandler);
                TimeSpan runTime = builder.Run();

                using (HttpServer? server = HttpServerFactory.CreateServerForTest(ServerLog, Path.Combine(WorkDirectory, _configuration.TargetWeb.OutPutDirectory)))
                {
                    server.Start();
                    Log.Info("-------------------------------------------------");
                    Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
                    Log.Info("Test server running on: http://localhost:8090/");
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
