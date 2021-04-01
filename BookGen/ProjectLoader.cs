//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain;
using BookGen.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BookGen
{
    internal class ProjectLoader
    {
        private readonly ILog _log;
        private readonly FsPath _configfile;
        private readonly string _workdir;

        public ProjectLoader(ILog log, string workdir)
        {
            _log = log;
            _workdir = workdir;
            _configfile = new FsPath(workdir, "bookgen.json");
        }

        public bool TryLoadAndValidateConfig(out Config? config)
        {
            config = null;

            if (!_configfile.IsExisting)
            {
                _log.Info("No bookgen.json config found.");
                return false;
            }

            config = _configfile.DeserializeJson<Config>(_log);

            if (config == null)
            {
                _log.Critical("bookgen.json deserialize error. Invalid config file");
                return false;
            }

            if (config.Version < Program.CurrentState.ConfigVersion)
            {
                _configfile.CreateBackup(_log);
                config.UpgradeTo(Program.CurrentState.ConfigVersion);
                _configfile.SerializeJson(config, _log, true);
                _log.Info("Configuration file migrated to new version.");
                _log.Info("Review configuration then run program again");
                return false;
            }

            ConfigValidator validator = new ConfigValidator(config, _workdir);
            validator.Validate();

            if (!validator.IsValid)
            {
                _log.Warning("Errors found in configuration: ");
                foreach (var error in validator.Errors)
                {
                    _log.Warning(error);
                }
                return false;
            }

            return true;
        }

        public bool TryLoadAndValidateToc(Config? config, out ToC? toc)
        {
            toc = null;

            if (config == null)
                return false;

            var tocFile = new FsPath(_workdir).Combine(config.TOCFile);
            _log.Info("Parsing TOC file...");
            
            toc = MarkdownUtils.ParseToc(tocFile.ReadFile(_log));

            _log.Info("Found {0} chapters and {1} files", toc.ChapterCount, toc.FilesCount);
            TocValidator tocValidator = new TocValidator(toc, _workdir);
            tocValidator.Validate();

            if (!tocValidator.IsValid)
            {
                _log.Warning("Errors found in TOC file: ");
                foreach (var error in tocValidator.Errors)
                {
                    _log.Warning(error);
                }
                return false;
            }

            _log.Info("Config file and TOC contain no errors");
            return true;
        }

        public RuntimeSettings CreateRuntimeSettings(Config config, ToC toc, BuildConfig current)
        {
            var settings = new RuntimeSettings
            {
                SourceDirectory = new FsPath(_workdir),
                Configuration = config,
                TocContents = toc,
                MetataCache = new Dictionary<string, string>(100),
                InlineImgCache = new ConcurrentDictionary<string, string>(),
                CurrentBuildConfig = current,
            };

            if (string.IsNullOrEmpty(config.ImageDir))
                settings.ImageDirectory = FsPath.Empty;
            else
                settings.ImageDirectory = settings.SourceDirectory.Combine(config.ImageDir);

            return settings;
        }

        public bool TryLoadProjectAndExecuteOperation(Func<Config, ToC, bool> operationToDo)
        {
            if (TryLoadAndValidateConfig(out Config? config)
                && TryLoadAndValidateToc(config, out ToC? toc)
                && config != null
                && toc != null)
            {
                return operationToDo(config, toc);
            }
            return false;
        }
    }
}
