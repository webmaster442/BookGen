//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using System;

namespace BookGen.Core
{
    public static class ConfigExtensions
    {
        public static void UpgradeTo(this Config config, int targetVersion)
        {
            if (config.Version == 0 || config.Version < targetVersion)
                config.Version = targetVersion;

            if (config.StyleClasses == null)
                config.StyleClasses = new StyleClasses();

            if (config.SearchOptions == null)
                config.SearchOptions = SearchSettings.CreateDefault();

            if (config.Metadata == null)
                config.Metadata = Metadata.CreateDefault();

            if (config.PrecompileHeader == null)
                config.PrecompileHeader = Precompile.CreateDefault();

            if (config.Assets == null)
                config.Assets = new System.Collections.ObjectModel.ObservableCollection<Asset>();
        }
    }
}
