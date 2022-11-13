//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Framework;
using BookGen.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BookGen.ProjectHandling
{
    internal class ProjectLoader
    {
        private enum ConfigMode
        {
            Yaml,
            Json,
            NotFound,
            Both
        }

        private readonly ILog _log;
        private readonly FsPath _configJson;
        private readonly FsPath _configYaml;
        private readonly FsPath _tags;
        private readonly string _workdir;

        public ProjectLoader(ILog log, string workdir)
        {
            _log = log;
            _workdir = workdir;
            _configJson = new FsPath(workdir, "bookgen.json");
            _configYaml = new FsPath(workdir, "bookgen.yml");
            _tags = new FsPath(workdir, "tags.json");
        }

        private ConfigMode GetConfigMode()
        {
            if (_configJson.IsExisting && _configYaml.IsExisting)
                return ConfigMode.Both;
            if (_configJson.IsExisting)
                return ConfigMode.Json;
            else if (_configYaml.IsExisting)
                return ConfigMode.Yaml;
            else
                return ConfigMode.NotFound;
        }

        public bool TryLoadAndValidateConfig([NotNullWhen(true)] out Config? config)
        {
            config = null;

            ConfigMode mode = GetConfigMode();

            switch (mode)
            {
                case ConfigMode.Yaml:
                    config = _configYaml.DeserializeYaml<Config>(_log);
                    break;
                case ConfigMode.Json:
                    config = _configJson.DeserializeJson<Config>(_log);
                    break;
                case ConfigMode.Both:
                    _log.Critical("both bookgen.json and bookgen.yml present. Decicde config format by deleting one of them");
                    return false;
                default:
                    _log.Critical("No bookgen.json config found.");
                    return false;
            }

            if (config == null)
            {
                _log.Critical("bookgen.json or boookgen.yml deserialize error. Invalid config file");
                return false;
            }

            if (config.Version < Program.CurrentState.ConfigVersion)
            {
                Upgrade(config, mode);
                _log.Info("Configuration file migrated to new version.");
                _log.Info("Review configuration then run program again");
                return false;
            }

            var validator = new ConfigValidator(config, _workdir);
            validator.Validate();

            if (!validator.IsValid)
            {
                _log.Warning("Errors found in configuration: ");
                foreach (string? error in validator.Errors)
                {
                    _log.Warning(error);
                }
                return false;
            }

            return true;
        }

        private void Upgrade(Config config, ConfigMode mode)
        {
            switch (mode)
            {
                case ConfigMode.Json:
                    _configJson.CreateBackup(_log);
                    break;
                case ConfigMode.Yaml:
                    _configYaml.CreateBackup(_log);
                    break;
            }
            config.UpgradeTo(Program.CurrentState.ConfigVersion);
            switch (mode)
            {
                case ConfigMode.Json:
                    _configJson.SerializeJson(config, _log, true);
                    break;
                case ConfigMode.Yaml:
                    _configYaml.SerializeYaml(config, _log);
                    break;
            }
        }

        public bool TryLoadAndValidateToc(Config config, [NotNullWhen(true)] out ToC? toc)
        {
            toc = null;

            if (config == null)
                return false;

            FsPath? tocFile = new FsPath(_workdir).Combine(config.TOCFile);
            _log.Info("Parsing TOC file...");

            toc = MarkdownUtils.ParseToc(tocFile.ReadFile(_log));

            _log.Info("Found {0} chapters and {1} files", toc.ChapterCount, toc.FilesCount);
            var tocValidator = new TocValidator(toc, _workdir);
            tocValidator.Validate();

            if (!tocValidator.IsValid)
            {
                _log.Warning("Errors found in TOC file: ");
                foreach (string? error in tocValidator.Errors)
                {
                    _log.Warning(error);
                }
                return false;
            }

            _log.Info("Config file and TOC contain no errors");
            return true;
        }

        public bool TryGetTags(CultureInfo culture, out TagUtils tagUtils)
        {
            if (_tags.IsExisting)
            {
                Dictionary<string, string[]>? deserialized = _tags.DeserializeJson<Dictionary<string, string[]>>(_log);
                if (deserialized != null)
                {
                    tagUtils = new TagUtils(deserialized, culture);
                    return true;
                }
                else
                {
                    _log.Critical("Invalid tags.json file. Continuing with empty collection");
                    tagUtils = new TagUtils(new(), culture);
                    return false;
                }
            }
            else
            {
                _log.Warning("tags.json not found, continuing with empty collection");
                tagUtils = new TagUtils(new(), culture);
                return true;
            }
        }

        public RuntimeSettings CreateRuntimeSettings(Config config, ToC toc, TagUtils tags, BuildConfig current)
        {
            var settings = new RuntimeSettings(tags)
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
                && TryLoadAndValidateToc(config, out ToC? toc))
            {
                return operationToDo(config, toc);
            }
            return false;
        }
    }
}
