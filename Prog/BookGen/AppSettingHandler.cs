//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO;
using System.Text.Json;

namespace BookGen
{
    public static class AppSettingHandler
    {
        private static string GetConfigFile()
        {
            string? path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(path, "BookGen.app.json");
        }

        public static AppSetting? LoadAppSettings()
        {
            string file = GetConfigFile();
            if (File.Exists(file))
            {
                string? contents = File.ReadAllText(file);
                return JsonSerializer.Deserialize<AppSetting>(contents);
            }
            else
            {
                return CreateDefaultSettings();
            }
        }

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

        public static void SaveAppSettings(AppSetting appSetting)
        {
            string file = GetConfigFile() + ".new";

            string? contents = JsonSerializer.Serialize<AppSetting>(appSetting, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            using (StreamWriter? stream = File.CreateText(file))
            {
                stream.Write(contents);
            }

            File.Move(file, GetConfigFile(), true);
        }

    }
}
