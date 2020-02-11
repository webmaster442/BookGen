//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Framework;
using BookGen.Framework.Editor;
using BookGen.Framework.Scripts;
using BookGen.Framework.Server;
using BookGen.GeneratorSteps;
using BookGen.Gui;
using BookGen.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace BookGen
{
    internal class GeneratorRunner
    {
        private readonly ILog _log;
        private readonly CsharpScriptHandler _scriptHandler;

        private const string exitString = "Press a key to exit...";

        public Config? Configuration { get; private set; }

        public FsPath ConfigFile { get; private set; }

        public string WorkDirectory
        {
            get;
        }

        public ILog Log => _log;

        public GeneratorRunner(ILog log, string workDir)
        {
            _log = log;
            _scriptHandler = new CsharpScriptHandler(_log);
            WorkDirectory = workDir;
            ConfigFile = new FsPath(WorkDirectory, "bookgen.json");
            Configuration = new Config();
        }

        internal void DoInteractiveInitialize()
        {
            var menu = new InteractiveInitializer(_log, new FsPath(WorkDirectory), Program.CurrentState);
            menu.Run();
        }

        public void RunHelp()
        {
            Console.WriteLine(HelpTextCreator.GenerateHelpText());
        }

        #region Helpers

        public void InitializeAndExecute(Action<GeneratorRunner> actionToExecute)
        {
            if (Initialize())
            {
                actionToExecute.Invoke(this);
            }
        }

        public bool Initialize(bool compileScripts = true)
        {
            _log.Info("---------------------------------------------------------");
            _log.Info("BookGen Build date: {0} Starting...", Program.CurrentState.BuildDate);
            _log.Info("Config API version: {0}", Program.CurrentState.ProgramVersion);
            _log.Info("Working directory: {0}", WorkDirectory);
            _log.Info("---------------------------------------------------------");

            if (!compileScripts)
            {
                return LoadAndValidateConfig()
                    && LoadAndValidateToc();
            }

            return LoadAndValidateConfig()
                   && LoadAndValidateToc()
                   && LoadAndCompileScripts();
        }

        private bool LoadAndValidateConfig()
        {

            if (!ConfigFile.IsExisting)
            {
                _log.Info("No bookgen.json config found.");
                Program.ShowMessageBox(exitString);
                return false;
            }

            Configuration = ConfigFile.DeserializeJson<Config>(_log);

            if (Configuration == null)
            {
                _log.Critical("bookgen.json deserialize error. Invalid config file");
                return false;
            }

            if (Configuration.Version < Program.CurrentState.ConfigVersion)
            {
                ConfigFile.CreateBackup(_log);
                Configuration.UpgradeTo(Program.CurrentState.ConfigVersion);
                ConfigFile.SerializeJson(Configuration, _log, true);
                _log.Info("Configuration file migrated to new version.");
                _log.Info("Review configuration then run program again");
                Program.ShowMessageBox(exitString);
                return false;
            }

            ConfigValidator validator = new ConfigValidator(Configuration, WorkDirectory);
            validator.Validate();

            if (!validator.IsValid)
            {
                _log.Warning("Errors found in configuration: ");
                foreach (var error in validator.Errors)
                {
                    _log.Warning(error);
                }
                Program.ShowMessageBox(exitString);
                return false;
            }

            return true;
        }

        private bool LoadAndValidateToc()
        {
            if (Configuration == null)
                return false;

            var tocFile = new FsPath(WorkDirectory).Combine(Configuration.TOCFile);
            _log.Info("Parsing TOC file...");
            var toc = MarkdownUtils.ParseToc(tocFile.ReadFile(_log));
            _log.Info("Found {0} chapters and {1} files", toc.ChapterCount, toc.FilesCount);
            TocValidator tocValidator = new TocValidator(toc, WorkDirectory);
            tocValidator.Validate();

            if (!tocValidator.IsValid)
            {
                _log.Warning("Errors found in TOC file: ");
                foreach (var error in tocValidator.Errors)
                {
                    _log.Warning(error);
                }
                Program.ShowMessageBox(exitString);
                return false;
            }

            _log.Info("Config file and TOC contain no errors");
            return true;
        }

        private bool LoadAndCompileScripts()
        {
            if (Configuration == null) return false;

            if (string.IsNullOrEmpty(Configuration.ScriptsDirectory)) return true;

            _log.Info("Trying to load and compile script files...");
            FsPath scripts = new FsPath(WorkDirectory).Combine(Configuration.ScriptsDirectory);

            int count = _scriptHandler.LoadScripts(scripts);
            _log.Info("Loaded {0} instances from script files", count);

            return true;
        }

        public void DoClean()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetWeb.OutPutDirectory), _log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetPrint.OutPutDirectory), _log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetEpub.OutPutDirectory), _log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetWordpress.OutPutDirectory), _log);
        }
        #endregion

        #region Argument handlers

        public void DoBuild()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            _log.Info("Building deploy configuration...");
            WebsiteBuilder builder = new WebsiteBuilder(WorkDirectory, Configuration, _log, _scriptHandler);
            var runTime = builder.Run();
            _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoPrint()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            _log.Info("Building print configuration...");
            PrintBuilder builder = new PrintBuilder(WorkDirectory, Configuration, _log, _scriptHandler);
            var runTime = builder.Run();
            _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoEpub()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            _log.Info("Building epub configuration...");
            EpubBuilder builder = new EpubBuilder(WorkDirectory, Configuration, _log, _scriptHandler);
            var runTime = builder.Run();
            _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoWordpress()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            _log.Info("Building Wordpress configuration...");
            WordpressBuilder builder = new WordpressBuilder(WorkDirectory, Configuration, _log, _scriptHandler);
            var runTime = builder.Run();
            _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoTest()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            _log.Info("Building test configuration...");
            Configuration.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(WorkDirectory, Configuration, _log, _scriptHandler);
            var runTime = builder.Run();

            using (var server = new HttpServer(Path.Combine(WorkDirectory, Configuration.TargetWeb.OutPutDirectory), 8080, _log))
            {
                _log.Info("-------------------------------------------------");
                _log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
                _log.Info("Test server running on: http://localhost:8080/");
                _log.Info("Serving from: {0}", Configuration.TargetWeb.OutPutDirectory);

                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = Configuration.HostName;
                p.Start();

                Console.WriteLine(exitString);
                Console.ReadLine();
            }
        }

        public void DoEditor()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            IRequestHandler[] handlers = new IRequestHandler[]
            {
                new DynamicHandlers(WorkDirectory, Configuration),
                new HtmlPageHandler(),
                new EmbededResourceRequestHandler()
            };

            using (var server = new HttpServer(WorkDirectory, 9090, _log, handlers))
            {
                _log.Info("Editor started on: http://localhost:9090");
                _log.Info("Press a key to exit...");

                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = "http://localhost:9090";
                p.Start();

                Console.ReadLine();
            }
        }

        #endregion
    }
}
