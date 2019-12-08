//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Controls;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BookGen.Editor.ServiceContracts
{
    public interface INHunspellServices
    {
        bool DownloadDictionaries(IList<string> codes, IProgress<float> progress, CancellationToken ct);
        bool DeleteDictionaries(IList<string> codes, IProgress<float> progress, CancellationToken ct);
        IEnumerable<string> GetAvailableLanguages();
        IEnumerable<string> GetInstalledLanguages();
        bool CreateConfiguredHunspell(string selectedLanguage, out Hunspell hunspell);
        string GetCurrentLanguage();
    }
}
