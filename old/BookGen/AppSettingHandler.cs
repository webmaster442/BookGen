//-----------------------------------------------------------------------------
// (c) 2020-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Threading;

using BookGen.Settings;

namespace BookGen;

public static class AppSettingHandler
{
    private static AppSetting CreateDefaultSettings()
    {
        var defaults = new AppSetting
        {
            EditorPath = EditorHelper.GetNotepadPath()
        };
        if (EditorHelper.TryFindVsCodeInstall(out string vsCode))
        {
            defaults.EditorPath = vsCode;
        }
        return defaults;
    }

    public static async Task SaveAppSettingsAsync(AppSetting appSetting, CancellationToken cancellationToken = default)
    {
        SettingsManager manager = FileProvider.GetSettingsManager();
        await manager.SerializeAsync(FileProvider.Keys.AppSettings, appSetting, cancellationToken);
    }

    public static async Task<AppSetting> LoadAppSettingsAsync(CancellationToken cancellationToken = default)
    {
        SettingsManager manager = FileProvider.GetSettingsManager();
        AppSetting? result = await manager.DeserializeAsync<AppSetting>(FileProvider.Keys.AppSettings, cancellationToken);
        
        if (result == null)
            return CreateDefaultSettings();

        return result;
    }

}
