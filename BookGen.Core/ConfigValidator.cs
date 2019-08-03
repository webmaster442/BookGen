//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Properties;

namespace BookGen.Core
{
    public class ConfigValidator: Validator
    {
        public ValidateFor ValidationLevel { get; set; }

        public enum ValidateFor
        {
            Web,
            Print,
            Epub,
            All,
        }

        private readonly Config _config;
        private readonly string _workdir;

        public ConfigValidator(Config config, string workdir)
        {
            ValidationLevel = ValidateFor.All;
            _config = config;
            _workdir = workdir;
        }

        private void ValidateBuildConfig(FsPath WorkDirectory, BuildConfig target)
        {
            if (target == null)
            {
                AddError(Resources.MissingSection);
                return;
            }

            if (!string.IsNullOrEmpty(target.TemplateFile)
                && !WorkDirectory.Combine(target.TemplateFile).IsExisting)
            {
                AddError(Resources.MissingTemplateFile, target.TemplateFile);
            }

            if (target.TemplateAssets == null)
                AddError(Resources.MissingAssets);

            foreach (var asset in target.TemplateAssets)
            {
                var source = WorkDirectory.Combine(asset.Source);

                if (!source.IsExisting)
                    AddError(Resources.MissingAsset, source.ToString());
            }

            if (target.StyleClasses == null)
                AddError(Resources.MissingStyleClasses);
        }

        public override void Validate()
        {
            if (Errors.Count > 0)
                Errors.Clear();

            var WorkDirectory = new FsPath(_workdir);

            switch (ValidationLevel)
            {
                case ValidateFor.Epub:
                    ValidateBuildConfig(WorkDirectory, _config.TargetEpub);
                    break;
                case ValidateFor.Print:
                    ValidateBuildConfig(WorkDirectory, _config.TargetPrint);
                    break;
                case ValidateFor.Web:
                    ValidateBuildConfig(WorkDirectory, _config.TargetWeb);
                    break;
                case ValidateFor.All:
                    ValidateBuildConfig(WorkDirectory, _config.TargetEpub);
                    ValidateBuildConfig(WorkDirectory, _config.TargetPrint);
                    ValidateBuildConfig(WorkDirectory, _config.TargetWeb);
                    break;
            }

            if (!string.IsNullOrEmpty(_config.ImageDir) && !WorkDirectory.Combine(_config.ImageDir).IsExisting)
                AddError(Resources.MissingImageDir, _config.ImageDir);

            if (!WorkDirectory.Combine(_config.TOCFile).IsExisting)
                AddError(Resources.MissingTocFile, _config.TOCFile);

            if (string.IsNullOrEmpty(_config.HostName))
            {
                AddError(Resources.MissingHostName);
            }
            else if (!_config.HostName.EndsWith("/"))
            {
                AddError(Resources.MissingTrailingSlash);
            }

            if (string.IsNullOrEmpty(_config.Index))
                AddError(Resources.MissingIndex);

            if (_config.SearchOptions == null)
                AddError(Resources.MissingSearchOptions);

            if (string.IsNullOrEmpty(_config.SearchOptions.NoResults)
                || string.IsNullOrEmpty(_config.SearchOptions.SearchButtonText)
                || string.IsNullOrEmpty(_config.SearchOptions.SearchPageTitle)
                || string.IsNullOrEmpty(_config.SearchOptions.SearchResults)
                || string.IsNullOrEmpty(_config.SearchOptions.SearchTextBoxText))
            {
                AddError(Resources.MissingSearchOptionsText);
            }

            if (_config.Metadata == null)
                AddError(Resources.MissingMetadata);
        }
    }
}
