//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.Models;
using Newtonsoft.Json;
using System;

namespace BookGen.Editor.Infrastructure
{
    public static class EditorSessionManager
    {
        public static EditorSession CurrentSession { get; private set; }

        private static FsPath _sessionFolder;
        private static FsPath _setttingsFile;

        static EditorSessionManager()
        {
            _sessionFolder = new FsPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BookGenEditor");
            _setttingsFile = _sessionFolder.Combine("EditorSession.json");
        }

        public static void Initialize()
        {
            if (_setttingsFile.IsExisting)
            {
                var content = _setttingsFile.ReadFile();
                CurrentSession = JsonConvert.DeserializeObject<EditorSession>(content);
            }
            else
            {
                CurrentSession = new EditorSession();
            }


            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            FsPath path = new FsPath(appdata, "BookGenEditor");
            if (!path.IsExisting)
                path.CreateDir(null);

            FsPath dictionaries = path.Combine("Dictionaries");
            if (!dictionaries.IsExisting)
                dictionaries.CreateDir(null);

            CurrentSession.DictionaryPath = dictionaries.ToString();
        }

        public static void Close()
        {
            var content = JsonConvert.SerializeObject(CurrentSession, Formatting.Indented);
            _setttingsFile.WriteFile(content);
        }
    }
}
