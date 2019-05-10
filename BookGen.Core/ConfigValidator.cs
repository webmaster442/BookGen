//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Properties;
using System.Collections.Generic;
using System.IO;

namespace BookGen.Core
{
    public class ConfigValidator
    {
        public List<string> Errors { get; }

        public ConfigValidator()
        {
            Errors = new List<string>();
        }

        private void AddError(string format, params string[] values)
        {
            Errors.Add(string.Format(format, values));
        }

        public bool Validate(Config config, string workdir)
        {
            if (Errors.Count > 0)
                Errors.Clear();

            var WorkDirectory = new FsPath(workdir);

            if (!WorkDirectory.Combine(config.Template).IsExisting)
                AddError(Resources.MissingTemplateFile, config.Template);

            if (!string.IsNullOrEmpty(config.ImageDir) && !WorkDirectory.Combine(config.ImageDir).IsExisting)
                AddError(Resources.MissingImageDir, config.ImageDir);

            if (!WorkDirectory.Combine(config.TOCFile).IsExisting)
                AddError(Resources.MissingTocFile, config.TOCFile);

            if (string.IsNullOrEmpty(config.HostName))
                AddError(Resources.MissingHostName);

            if (config.Assets == null)
                AddError(Resources.MissingAssets);
                
            foreach (var asset in config.Assets)
            {
                if (!File.Exists(asset.Source))
                    AddError(Resources.MissingAsset, asset.Source);
            }

            if (string.IsNullOrEmpty(config.Index))
                AddError(Resources.MissingIndex);

            if (config.StyleClasses == null)
                AddError(Resources.MissingStyleClasses);

            if (config.SearchOptions == null)
                AddError(Resources.MissingSearchOptions);

            if (string.IsNullOrEmpty(config.SearchOptions.NoResults)
                || string.IsNullOrEmpty(config.SearchOptions.SearchButtonText)
                || string.IsNullOrEmpty(config.SearchOptions.SearchPageTitle)
                || string.IsNullOrEmpty(config.SearchOptions.SearchResults)
                || string.IsNullOrEmpty(config.SearchOptions.SearchTextBoxText))
                AddError(Resources.MissingSearchOptionsText);


            if (config.Metadata == null)
                AddError(Resources.MissingMetadata);

            if (config.PrecompileHeader == null)
                AddError(Resources.MissingPrecompile);

            return Errors.Count == 0;
        }

    }
}
