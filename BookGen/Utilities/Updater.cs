//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Domain;
using BookGen.Resources;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookGen.Utilities
{
    public class Updater
    {
        private const string UpdateUrl = "";
        private readonly ILog _log;
        private readonly Version _currentBuild;
        private readonly string _appDir;

        public Updater(ILog log, DateTime currentBuild, string appDir)
        {
            _log = log;
            _currentBuild = new Version(currentBuild.Year, currentBuild.Month, currentBuild.Year);
            _appDir = appDir;
        }

        private async Task<Release[]> GetReleases()
        {
            using (var client = new WebClient())
            {
                client.UseDefaultCredentials = true;
                client.Proxy = WebRequest.GetSystemWebProxy();
                var json = await client.DownloadStringTaskAsync(new Uri(UpdateUrl));
                var result = JsonSerializer.Deserialize<Release[]>(json);
                if (result == null)
                    throw new InvalidOperationException("Error while deserializing update info...");
                
                return result;
            }
        }

        public async Task<Version?> GetLatestVersion(bool preview = false)
        {
            var releases = await GetReleases();
            return releases
                .OrderByDescending(x => x.Version)
                .FirstOrDefault(x => x.IsPreview == preview)
                ?.Version;
        }

        public bool IsUpdateNewerThanCurrentVersion(Version? updateVersion)
        {
            if (updateVersion == null)
                return false;

            return updateVersion > _currentBuild;
        }

        public void LaunchUpdateScript()
        {
            var updater = ResourceHandler.GetResourceFile<KnownFile>("Powershell/completer.ps1");

            FsPath script = new FsPath(_appDir, "updater.ps1");
            if (script.WriteFile(_log, updater))
            {
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -File \"{script}\"";
                    process.Start();
                }
            }
        }
    }
}
