//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using BookGen.Core.Configuration;

namespace BookGen.Core
{
    public static class ConfigExtensions
    {
        public static void UpgradeTo(this Config config, int targetVersion)
        {
            if (config.Version == 0 || config.Version < targetVersion)
                config.Version = targetVersion;

            if (config.Translations == null)
            {
                config.Translations = Translations.CreateDefault();
            }

            UpgradeBuildTarget(config.TargetWeb);
            UpgradeBuildTarget(config.TargetEpub);
            UpgradeBuildTarget(config.TargetPrint);
        }

        private static void UpgradeBuildTarget(BuildConfig target)
        {
            if (target.TemplateOptions == null)
            {
                target.TemplateOptions = TemplateOptions.CreateDefaultOptions();
            }
        }
    }
}
