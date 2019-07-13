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
        private readonly Config _config;
        private readonly string _workdir;

        public ConfigValidator(Config config, string workdir)
        {
            _config = config;
            _workdir = workdir;
        }

        public override void Validate()
        {
            if (Errors.Count > 0)
                Errors.Clear();

            var WorkDirectory = new FsPath(_workdir);

            if (!WorkDirectory.Combine(_config.Template).IsExisting)
                AddError(Resources.MissingTemplateFile, _config.Template);

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

            if (_config.Assets == null)
                AddError(Resources.MissingAssets);
                
            foreach (var asset in _config.Assets)
            {
                var source = WorkDirectory.Combine(asset.Source);

                if (!source.IsExisting)
                    AddError(Resources.MissingAsset, source.ToString());
            }

            if (string.IsNullOrEmpty(_config.Index))
                AddError(Resources.MissingIndex);

            if (_config.StyleClasses == null)
                AddError(Resources.MissingStyleClasses);

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

            if (_config.PrecompileHeader == null)
                AddError(Resources.MissingPrecompile);
        }
    }
}
