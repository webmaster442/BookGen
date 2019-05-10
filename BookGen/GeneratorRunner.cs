//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework.Server;
using BookGen.GeneratorSteps;
using BookGen.Utilities;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BookGen
{
    internal class GeneratorRunner
    {
        private readonly ArgsumentList _arguments;
        private readonly ILog _log;
        private FsPath _config;
        private Config _cfg;
        private string _workdir;

        public GeneratorRunner(string[] arguments, ILog log)
        {
            _arguments = ArgsumentList.Parse(arguments);
            _log = log;
        }

        public void RunGenerator()
        {
            var action = _arguments.GetArgument("a", "action");
            _workdir = _arguments.GetArgument("d", "dir")?.Value;

            if (action == null) RunHelp();
            if (_workdir == null) _workdir = Environment.CurrentDirectory;

            var logfile = Path.Combine(_workdir, "bookgen.log");
            _log.ConfigureFile(logfile);
            _config = new FsPath(_workdir, "bookgen.json");

            switch (action.Value)
            {
                case KnownArguments.BuildWeb:
                    if (!Initialize(_workdir)) return;
                    DoBuild();
                    break;
                case KnownArguments.Clean:
                    if (!Initialize(_workdir)) return;
                    CreateOutputDirectory.CleanDirectory(new FsPath(_cfg.OutputDir), _log);
                    break;
                case KnownArguments.TestWeb:
                    if (!Initialize(_workdir)) return;
                    DoTest();
                    break;
                case KnownArguments.BuildPrint:
                    if (!Initialize(_workdir)) return;
                    DoPrint();
                    break;
                case KnownArguments.CreateConfig:
                    DoCreateConfig();
                    break;
                case KnownArguments.ValidateConfig:
                    Initialize(_workdir);
                    PressKeyToExit();
                    break;
                case KnownArguments.BuildEpub:
                    if (!Initialize(_workdir)) return;
                    DoEpub();
                    break;
                default:
                    RunHelp();
                    break;
            }
        }

        private void RunHelp()
        {
            Console.WriteLine(Properties.Resources.Help);
            Environment.Exit(1);
        }

        #region Helpers
        private bool Initialize(string workdir)
        {
            Version version = GetVersion();
            Splash.DoSplash();
            _log.Info("BookGen V{0} Starting...", version);
            _log.Info("Working directory: {0}", workdir);
            _log.Info("---------------------------------------------------------\n\n");
            int cfgVersion = (version.Major * 100) + version.Minor;

            if (!_config.IsExisting)
            {
                _log.Info("No bookgen.json config found.");
                PressKeyToExit();
                return false;
            }

            var cfgstring = _config.ReadFile();

            _log.Detail("Configuration content: {0}", cfgstring);

            _cfg = JsonConvert.DeserializeObject<Config>(cfgstring);

            if (_cfg.Version < cfgVersion)
            {
                _cfg.UpgradeTo(cfgVersion);
                WriteConfig(_config, _cfg);
                _log.Info("Configuration file migrated to new version.");
                _log.Info("Review configuration then run program again");
                PressKeyToExit();
                return false;
            }

            ConfigValidator validator = new ConfigValidator(_cfg, _workdir);
            validator.Validate();
            
            if (!validator.IsValid)
            {
                Console.WriteLine("Errors found in configuration: ");
                foreach (var error in validator.Errors)
                {
                    Console.WriteLine(error);
                }
                PressKeyToExit();
                return false;
            }
            else
            {
                var tocFile = new FsPath(workdir).Combine(_cfg.TOCFile);
                var toc = MarkdownUtils.ParseToc(tocFile.ReadFile());
                TocValidator tocValidator = new TocValidator(toc, workdir);
                tocValidator.Validate();
                if (!tocValidator.IsValid)
                {
                    Console.WriteLine("Errors found in TOC file: ");
                    foreach (var error in tocValidator.Errors)
                    {
                        Console.WriteLine(error);
                    }
                    PressKeyToExit();
                    return false;
                }
                else
                {
                    Console.WriteLine("Config file contains no errors");
                }
            }

            return true;
        }

        private static Version GetVersion()
        {
            var asm = Assembly.GetAssembly(typeof(Program));
            return asm.GetName().Version;
        }

        private static void WriteConfig(FsPath configFile, Config configuration)
        {
            var def = JsonConvert.SerializeObject(configuration, Formatting.Indented);
            configFile.WriteFile(def);
        }

        private static void PressKeyToExit()
        {
            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }
        #endregion

        #region Argument handlers

        private void DoCreateConfig()
        {
            _log.Info("Creating default config file: {0}", _config.ToString());
            WriteConfig(_config, Config.CreateDefault());
        }

        private void DoBuild()
        {
            _log.Info("Building deploy configuration...");
            WebsiteBuilder builder = new WebsiteBuilder(_workdir, _cfg, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        private void DoPrint()
        {
            _log.Info("Building print configuration...");
            PrintBuilder builder = new PrintBuilder(_workdir, _cfg, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }

        private void DoEpub()
        {
            _log.Info("Building epub configuration...");
            EpubBuilder builder = new EpubBuilder(_workdir, _cfg, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
        }


        private void DoTest()
        {
            _log.Info("Building test configuration...");
            _cfg.HostName = "http://localhost:8080/";
            WebsiteBuilder builder = new WebsiteBuilder(_workdir, _cfg, _log);
            var runTime = builder.Run();
            _log.Info("Runtime: {0}", runTime);
            using (var server = new HTTPTestServer(Path.Combine(_workdir, _cfg.OutputDir), 8080, _log))
            {
                Console.Clear();
                _log.Info("Test server running on: http://localhost:8080/");
                _log.Info("Serving from: {0}", _cfg.OutputDir);
                Process.Start(_cfg.HostName);
                Splash.PressKeyToExit();
            }
        }

        #endregion
    }
}
