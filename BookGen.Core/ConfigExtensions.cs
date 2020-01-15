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
    }
}
