﻿//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using System;
using System.IO;
using System.Text.Json;

namespace BookGen
{
    public static class AppSettingHandler
    {
        private static string GetConfigFile()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var file = Path.Combine(path, "BookGen.app.json");
            return file;
        }

        public static AppSetting? LoadAppSettings()
        {
            string file = GetConfigFile();
            if (File.Exists(file))
            {
                var contents = File.ReadAllText(file);
                return JsonSerializer.Deserialize<AppSetting>(contents);
            }
            else
            {
                return new AppSetting();
            }
        }

        public static void SaveAppSettings(AppSetting appSetting)
        {
            string file = GetConfigFile()+".new";

            var contents = JsonSerializer.Serialize<AppSetting>(appSetting, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            using (var stream = File.CreateText(file))
            {
                stream.Write(contents);
            }

            File.Move(file, GetConfigFile(), true);
        }

    }
}
