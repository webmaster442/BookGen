//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.Controls;
using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace BookGen.Editor.Services
{
    internal class NHuspellServices: INHunspellServices
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        private const string BaseUrl = "https://raw.githubusercontent.com/titoBouzout/Dictionaries/master/{0}.{1}";
#pragma warning restore S1075 // URIs should not be hardcoded

        public bool DownloadDictionaries(IList<string> codes, IProgress<float> progress, CancellationToken ct)
        {
            float maxitems = codes.Count * 2.0f;
            int done = 0;
            using (WebClient client = new WebClient())
            {
                foreach (var code in codes)
                {
                    if (ct.IsCancellationRequested) return false;
                    var aff = string.Format(BaseUrl, code, "aff");
                    var dic = string.Format(BaseUrl, code, "dic");

                    var affTarget = Path.Combine(EditorSessionManager.CurrentSession.DictionaryPath, $"{code}.aff");
                    var dicTarget = Path.Combine(EditorSessionManager.CurrentSession.DictionaryPath, $"{code}.dic");

                    client.DownloadFile(aff, affTarget);
                    done++;
                    progress.Report(done / maxitems);
                    if (ct.IsCancellationRequested) return false;
                    client.DownloadFile(dic, dicTarget);
                    done++;
                    progress.Report(done / maxitems);
                    if (ct.IsCancellationRequested) return false;
                }
            }

            return true;
        }

        public bool DeleteDictionaries(IList<string> codes, IProgress<float> progress, CancellationToken ct)
        {
            int done = 0;
            foreach (var code in codes)
            {
                if (ct.IsCancellationRequested) return false;

                var affTarget = Path.Combine(EditorSessionManager.CurrentSession.DictionaryPath, $"{code}.aff");
                var dicTarget = Path.Combine(EditorSessionManager.CurrentSession.DictionaryPath, $"{code}.dic");

                if (File.Exists(affTarget))
                    File.Delete(affTarget);
                if (File.Exists(dicTarget))
                    File.Delete(dicTarget);

                done++;
                progress.Report(done / (codes.Count * 2.0f));
            }

            return true;
        }

        public IEnumerable<string> GetAvailableLanguages()
        {
            yield return "Armenian (Eastern)";
            yield return "Armenian (Western)";
            yield return "Basque";
            yield return "Bulgarian";
            yield return "Catalan";
            yield return "Croatian";
            yield return "Czech";
            yield return "Danish";
            yield return "Dutch";
            yield return "English (American)";
            yield return "English (Australian)";
            yield return "English (British)";
            yield return "English (Canadian)";
            yield return "English (South African)";
            yield return "Estonian";
            yield return "French";
            yield return "Galego";
            yield return "German_de_AT";
            yield return "German_de_CH";
            yield return "German_de_DE";
            yield return "German_de_DE_OLDSPELL";
            yield return "Greek";
            yield return "Hungarian";
            yield return "Icelandic";
            yield return "Indonesia";
            yield return "Italian";
            yield return "Korean";
            yield return "Latvian";
            yield return "Lithuanian";
            yield return "Luxembourgish";
            yield return "Malays";
            yield return "Mongolian";
            yield return "Norwegian (Bokmal)";
            yield return "Norwegian (Nynorsk)";
            yield return "Persian";
            yield return "Polish";
            yield return "Portuguese (Brazilian)";
            yield return "Portuguese (European - Before OA 1990)";
            yield return "Portuguese (European)";
            yield return "Romanian (Ante1993)";
            yield return "Romanian (Modern)";
            yield return "Russian-English Bilingual";
            yield return "Russian";
            yield return "Serbian (Cyrillic)";
            yield return "Serbian (Latin)";
            yield return "Slovak_sk_SK";
            yield return "Slovenian";
            yield return "Spanish";
            yield return "Swedish";
            yield return "Turkish";
            yield return "Ukrainian_uk_UA";
            yield return "Vietnamese_vi_VN";
            yield return "be-official";
        }

        public bool CreateConfiguredHunspell(string selectedLanguage, out Hunspell hunspell)
        {
            FsPath aff = new FsPath(EditorSessionManager.CurrentSession.DictionaryPath, $"{selectedLanguage}.aff");
            FsPath dic = new FsPath(EditorSessionManager.CurrentSession.DictionaryPath, $"{selectedLanguage}.dic");

            if (aff.IsExisting && dic.IsExisting)
            {
                hunspell = new Hunspell(aff.ToString(), dic.ToString());
                return true;
            }
            hunspell = null;
            return false;
        }

        public IEnumerable<string> GetInstalledLanguages()
        {
            var files = System.IO.Directory.GetFiles(EditorSessionManager.CurrentSession.DictionaryPath, "*.aff");
            return files.Select(f => System.IO.Path.GetFileNameWithoutExtension(f));
        }

        public string GetCurrentLanguage()
        {
            return Properties.Settings.Default.Editor_SpellCheckLanguage;
        }
    }
}
