//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Domain;
using System.Net;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class PrecompileHeader : IGeneratorContentFillStep
    {
        public GeneratorContent Content { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating & precompiling header...");
            StringBuilder compiled = new StringBuilder();
            foreach (var css in settings.Configruation.PrecompileHeader.CSSFiles)
            {
                log.Detail("Compiling into header: {0}", css);
                var content = GetFileContents(css, settings.SourceDirectory);
                compiled.AppendFormat("<style type=\"text/css\">{0}</style>\n", content);
            }
            foreach (var js in settings.Configruation.PrecompileHeader.JavascriptFiles)
            {
                log.Detail("Compiling into header: {0}", js);
                var content = GetFileContents(js, settings.SourceDirectory);
                compiled.AppendFormat("<script type=\"text/javascript\">{0}</script>\n", content);
            }
            Content.PrecompiledHeader = compiled.ToString();
        }

        private string GetFileContents(string url, FsPath sourceDirectory)
        {
            return url.StartsWith("https://")
                || url.StartsWith("http://")
                ? DownloadFile(url)
                : sourceDirectory.Combine(url).ReadFile();
        }

        private string DownloadFile(string url)
        {
            using (var client = new WebClient())
            {
                IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
                if (defaultProxy != null)
                {
                    defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                    client.Proxy = defaultProxy;
                }
                return client.DownloadString(url);
            }
        }
    }
}
