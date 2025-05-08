//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Settings;

public static class FileProvider
{
    private const string SettingsManagerFile = "bookgen.settings";

    public static class Keys
    {
        public const string AppSettings = "appsettings";
        public const string TodoItems = "todoitems";
        public const string Launcher = "launcher";
    }

    public static SettingsManager GetSettingsManager()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var file = Path.Combine(userProfile, SettingsManagerFile);
        return new SettingsManager(file);
    }

    public static string GetWwwConfig()
        => Path.Combine(AppContext.BaseDirectory, "WwwConfig.xml");
}
