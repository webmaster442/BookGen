//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NHunspell;

namespace BookGen.Editor.ServiceContracts
{
    public interface INHunspellServices
    {
        Task DownloadDictionary(string code, string target);
        IEnumerable<string> GetLanguages();
        Hunspell CreateConfiguredHunspell(string selectedLanguage);
    }
}
