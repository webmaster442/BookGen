//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using System;
using System.Net;

namespace BookGen.Utilities
{
    internal class DictionaryManager
    {
        private const string BaseUrl = "https://raw.githubusercontent.com/freedesktop/libreoffice-dictionaries/master/{0}/{0}.{1}";
        private const int BufferSize = 4 * 1024;
        private readonly ILog _log;
        private readonly IAppSetting _appSetting;

        public DictionaryManager(ILog log, IAppSetting appSetting)
        {
            _log = log;
            _appSetting = appSetting;
        }

        public void InstallDictionary(string languageCode)
        {
            _log.Info("Installing dictionaries for: {0}", languageCode);
            var aff = string.Format(BaseUrl, languageCode, "aff");
            var dic = string.Format(BaseUrl, languageCode, "dic");
            FsPath appdata = new FsPath(_appSetting.AppDataPath);

            DownloadFile(aff, appdata.Combine($"{languageCode}.aff"));
            DownloadFile(dic, appdata.Combine($"{languageCode}.dic"));
        }

        public void UninstallDictionary(string? languageCode = null)
        {
            if (languageCode == null)
            {
                FsPath appdata = new FsPath(_appSetting.AppDataPath);
                var affs = appdata.GetAllFiles("*.aff");
                var dics = appdata.GetAllFiles("*.dic");
                foreach (var aff in affs) aff.Delete(_log);
                foreach (var dic in dics) dic.Delete(_log);
            }
            else
            {
                FsPath appdata = new FsPath(_appSetting.AppDataPath);
                var aff = appdata.Combine($"{languageCode}.aff");
                var dic = appdata.Combine($"{languageCode}.dic");
                aff.Delete(_log);
                dic.Delete(_log);
            }

        }

        private void DownloadFile(string source, FsPath destination)
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var sourceStream = client.OpenRead(source))
                    {
                        long total = 0;
                        using (var destStream = destination.CreateStream(_log))
                        {
                            var buffer = new byte[BufferSize];
                            int read = 0;
                            do
                            {
                                read = sourceStream.Read(buffer, 0, buffer.Length);
                                destStream.Write(buffer, 0, read);
                                total += read;
                                _log.Info("Downloaded: {0}\r", total);
                            }
                            while (read > 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Warning("Couldn't download dictionaries");
                _log.Critical(ex);
            }
        }
    }
}
