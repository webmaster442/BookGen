//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;

namespace BookGen.DomainServices
{
    public static class ConfigExtensions
    {
        public static void UpgradeTo(this Config config, int targetVersion)
        {
            if (config.Version == 0 || config.Version < targetVersion)
                config.Version = targetVersion;

            config.Translations ??= Translations.CreateDefault();

            UpgradeTranslations(config.Translations);
        }

        private static void UpgradeTranslations(Translations translations)
        {
            var defaults = Translations.CreateDefault();
            foreach (KeyValuePair<string, string> item in defaults)
            {
                if (!translations.ContainsKey(item.Key))
                {
                    translations.Add(item.Key, item.Value);
                }
            }
        }
    }
}
