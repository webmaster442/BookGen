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

        private string FixLanguageCode(string languageCode)
        {
            const string error = "Invalid Language code";

            var parts = languageCode.Split('_');
            if (parts.Length < 2)
                throw new ArgumentException(error);

            if (parts[0].Length != 2
                || parts[1].Length != 2)
                throw new ArgumentException(error);

            return $"{parts[0]}_{parts[1].ToUpper()}";

        }

        public void InstallDictionary(string languageCode)
        {
            try
            {

                languageCode = FixLanguageCode(languageCode);

                _log.Info("Installing dictionaries for: {0}", languageCode);
                var aff = string.Format(BaseUrl, languageCode, "aff");
                var dic = string.Format(BaseUrl, languageCode, "dic");
                FsPath appdata = new FsPath(_appSetting.AppDataPath);

                DownloadFile(aff, appdata.Combine($"{languageCode}.aff"));
                DownloadFile(dic, appdata.Combine($"{languageCode}.dic"));

                _log.Info("Installed dictionary: {0}", languageCode);
            }
            catch (Exception ex)
            {
                _log.Warning("Couldn't install dictionary");
                _log.Critical(ex);
            }
        }

        public void UninstallDictionary(string? languageCode = null)
        {
            try
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
                    languageCode = FixLanguageCode(languageCode);
                    FsPath appdata = new FsPath(_appSetting.AppDataPath);
                    var aff = appdata.Combine($"{languageCode}.aff");
                    var dic = appdata.Combine($"{languageCode}.dic");
                    aff.Delete(_log);
                    dic.Delete(_log);
                }
            }
            catch (Exception ex)
            {
                _log.Warning("Couldn't uninstall dictionary");
                _log.Critical(ex);
            }
        }

        private void DownloadFile(string source, FsPath destination)
        {
            using var client = new WebClient();
            using var sourceStream = client.OpenRead(source);
            long total = 0;
            using var destStream = destination.CreateStream(_log);
            var buffer = new byte[BufferSize];
            int read = 0;
            DateTime lastReport = DateTime.Now;
            do
            {
                read = sourceStream.Read(buffer, 0, buffer.Length);
                destStream.Write(buffer, 0, read);
                total += read;
                if ((DateTime.Now - lastReport).TotalSeconds > 0.5)
                {
                    _log.Info("Downloaded: {0} bytes", total);
                    lastReport = DateTime.Now;
                }
            }
            while (read > 0);
        }
    }
}
