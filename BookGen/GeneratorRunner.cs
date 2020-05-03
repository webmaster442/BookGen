//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Framework.Editor;
using BookGen.Framework.Scripts;
using BookGen.Framework.Server;
using BookGen.GeneratorSteps;
using BookGen.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace BookGen
{
    internal class GeneratorRunner
    {
        private readonly CsharpScriptHandler _scriptHandler;

        private const string exitString = "Press a key to exit...";

        public Config? Configuration { get; private set; }

        public FsPath ConfigFile { get; private set; }

        public string WorkDirectory
        {
            get;
        }

        public ILog Log { get; }

        public GeneratorRunner(ILog log, string workDir)
        {
            Log = log;
            _scriptHandler = new CsharpScriptHandler(Log);
            WorkDirectory = workDir;
            ConfigFile = new FsPath(WorkDirectory, "bookgen.json");
            Configuration = new Config();
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
        }

        public bool Initialize(bool compileScripts = true)
        {
            Log.Info("---------------------------------------------------------");
            Log.Info("BookGen Build date: {0:yyyy:MM:dd} Starting...", Program.CurrentState.BuildDate.Date);
            Log.Info("Config API version: {0}", Program.CurrentState.ProgramVersion);
            Log.Info("Working directory: {0}", WorkDirectory);
            Log.Info("---------------------------------------------------------");

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
                Log.Info("No bookgen.json config found.");
                Program.ShowMessageBox(exitString);
                return false;
            }

            Configuration = ConfigFile.DeserializeJson<Config>(Log);

            if (Configuration == null)
            {
                Log.Critical("bookgen.json deserialize error. Invalid config file");
                return false;
            }

            if (Configuration.Version < Program.CurrentState.ConfigVersion)
            {
                ConfigFile.CreateBackup(Log);
                Configuration.UpgradeTo(Program.CurrentState.ConfigVersion);
                ConfigFile.SerializeJson(Configuration, Log, true);
                Log.Info("Configuration file migrated to new version.");
                Log.Info("Review configuration then run program again");
                Program.ShowMessageBox(exitString);
                return false;
            }

            ConfigValidator validator = new ConfigValidator(Configuration, WorkDirectory);
            validator.Validate();

            if (!validator.IsValid)
            {
                Log.Warning("Errors found in configuration: ");
                foreach (var error in validator.Errors)
                {
                    Log.Warning(error);
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
            Log.Info("Parsing TOC file...");
            var toc = MarkdownUtils.ParseToc(tocFile.ReadFile(Log));
            Log.Info("Found {0} chapters and {1} files", toc.ChapterCount, toc.FilesCount);
            TocValidator tocValidator = new TocValidator(toc, WorkDirectory);
            tocValidator.Validate();

            if (!tocValidator.IsValid)
            {
                Log.Warning("Errors found in TOC file: ");
                foreach (var error in tocValidator.Errors)
                {
                    Log.Warning(error);
                }
                Program.ShowMessageBox(exitString);
                return false;
            }

            Log.Info("Config file and TOC contain no errors");
            return true;
        }

        private bool LoadAndCompileScripts()
        {
            if (Configuration == null) return false;

            if (string.IsNullOrEmpty(Configuration.ScriptsDirectory)) return true;

            Log.Info("Trying to load and compile script files...");
            FsPath scripts = new FsPath(WorkDirectory).Combine(Configuration.ScriptsDirectory);

            int count = _scriptHandler.LoadScripts(scripts);
            Log.Info("Loaded {0} instances from script files", count);

            return true;
        }

        public void DoClean()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetWeb.OutPutDirectory), Log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetPrint.OutPutDirectory), Log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetEpub.OutPutDirectory), Log);
            CreateOutputDirectory.CleanDirectory(new FsPath(Configuration.TargetWordpress.OutPutDirectory), Log);
        }
        #endregion

        #region Argument handlers

        public void DoBuild()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            Log.Info("Building deploy configuration...");
            WebsiteBuilder builder = new WebsiteBuilder(WorkDirectory, Configuration, Log, _scriptHandler);
            var runTime = builder.Run();
            Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoPrint()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            Log.Info("Building print configuration...");
            PrintBuilder builder = new PrintBuilder(WorkDirectory, Configuration, Log, _scriptHandler);
            var runTime = builder.Run();
            Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoEpub()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            Log.Info("Building epub configuration...");
            EpubBuilder builder = new EpubBuilder(WorkDirectory, Configuration, Log, _scriptHandler);
            var runTime = builder.Run();
            Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoWordpress()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            Log.Info("Building Wordpress configuration...");
            WordpressBuilder builder = new WordpressBuilder(WorkDirectory, Configuration, Log, _scriptHandler);
            var runTime = builder.Run();
            Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
        }

        public void DoTest()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            Log.Info("Building test configuration...");
            Configuration.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(WorkDirectory, Configuration, Log, _scriptHandler);
            var runTime = builder.Run();

            using (var server = new HttpServer(Path.Combine(WorkDirectory, Configuration.TargetWeb.OutPutDirectory), 8080, Log))
            {
                Log.Info("-------------------------------------------------");
                Log.Info("Runtime: {0:0.000} ms", runTime.TotalMilliseconds);
                Log.Info("Test server running on: http://localhost:8080/");
                Log.Info("Serving from: {0}", Configuration.TargetWeb.OutPutDirectory);

                if (Program.AppSetting.AutoStartWebserver)
                {
                    StartUrl(Configuration.HostName);
                }

                Console.WriteLine(exitString);
                Console.ReadLine();
            }
        }

        private void StartUrl(string url)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = url;
            p.Start();
        }

        public void DoEditor()
        {
            if (Configuration == null)
                throw new InvalidOperationException("Configuration is null");

            IRequestHandler[] handlers = new IRequestHandler[]
            {
                new DynamicHandlers(WorkDirectory, Configuration),
                new HtmlPageHandler(),
                new EmbededResourceRequestHandler(),
                new RunBookGenHandler(WorkDirectory),
            };

            using (var server = new HttpServer(WorkDirectory, 9090, Log, handlers))
            {
                Log.Info("Editor started on: http://localhost:9090");
                Log.Info("Press a key to exit...");

                if (Program.AppSetting.AutoStartWebserver)
                {
                    StartUrl("http://localhost:9090");
                }

                Console.ReadLine();
            }
        }

        #endregion
    }
}
