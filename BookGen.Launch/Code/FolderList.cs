using BookGen.Launch.Properties;
using System.Collections.Generic;
using System.Text.Json;

namespace BookGen.Launch.Code
{
    internal static class FolderList
    {
        public static IList<string> GetFolders()
        {
            if (string.IsNullOrEmpty(Settings.Default.FolderListJson))
                return new List<string>();

            string[]? deserialized = JsonSerializer.Deserialize<string[]>(Settings.Default.FolderListJson);
            if (deserialized != null)
                return deserialized;

            return new List<string>();
        }

        public static void SaveFolders(IList<string> folders)
        {
            var text = JsonSerializer.Serialize(folders);
            Settings.Default.FolderListJson = text;
            Settings.Default.Save();
        }

    }
}
