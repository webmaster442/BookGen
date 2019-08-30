//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using NHunspell;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BookGen.Editor.Services
{
    internal class NHuspellServices: INHunspellServices
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        private const string BaseUrl = "https://raw.githubusercontent.com/titoBouzout/Dictionaries/master/{0}.{1}";
#pragma warning restore S1075 // URIs should not be hardcoded

        public async Task DownloadDictionary(string code, string target)
        {
            using (var client = new WebClient())
            {
                var aff = string.Format(BaseUrl, code, "aff");
                var dic = string.Format(BaseUrl, code, "dic");
                await client.DownloadFileTaskAsync(aff, target).ConfigureAwait(false);
                await client.DownloadFileTaskAsync(dic, target).ConfigureAwait(false);
            }
        }

        public IEnumerable<string> GetLanguages()
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

        public Hunspell CreateConfiguredHunspell(string selectedLanguage)
        {
            FsPath aff = new FsPath(EditorSessionManager.CurrentSession.DictionaryPath, $"{selectedLanguage}.aff");
            FsPath dic = new FsPath(EditorSessionManager.CurrentSession.DictionaryPath, $"{selectedLanguage}.dic");

            return new Hunspell(aff.ToString(), dic.ToString());
        }
    }
}
