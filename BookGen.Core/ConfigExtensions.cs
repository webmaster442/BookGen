//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

            UpgradeTranslations(config.Translations);

            UpgradeBuildTarget(config.TargetWeb);
            UpgradeBuildTarget(config.TargetEpub);
            UpgradeBuildTarget(config.TargetPrint);

            if (config.TargetWordpress == null)
                config.TargetWordpress = BuildConfig.CreateDefault(ConfigurationFactories.CreateWordpressOptions());
        }

        private static void UpgradeTranslations(Translations translations)
        {
            var defaults = Translations.CreateDefault();
            foreach (var item in defaults)
            {
                if (!translations.ContainsKey(item.Key))
                {
                    translations.Add(item.Key, item.Value);
                }
            }
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
