//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
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

        public Updater(ILog log, DateTime currentBuild)
        {
            _log = log;
            _currentBuild = new Version(currentBuild.Year, currentBuild.Month, currentBuild.Year);
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

        public async Task<Version?> GetLatestVersion()
        {
            var releases = await GetReleases();
            return releases
                .OrderByDescending(x => x.Version)
                .Select(x => x.Version)
                .FirstOrDefault();
        }

        public bool IsUpdateNewerThanCurrentVersion(Version? updateVersion)
        {
            if (updateVersion == null)
                return false;

            return updateVersion > _currentBuild;
        }

        public void LaunchUpdateScript()
        {
            //todo: Launch powershell update script
        }
    }
}
