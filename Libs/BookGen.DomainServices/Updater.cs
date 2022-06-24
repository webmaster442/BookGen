﻿//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using BookGen.Interfaces;
using BookGen.Resources;
using System.Text.Json;

namespace BookGen.DomainServices
{
    public class Updater
    {
        private const string UpdateUrl = "https://raw.githubusercontent.com/webmaster442/BookGen/master/.github/updates.json";

        private readonly ILog _log;
        private readonly Version _currentBuild;
        private readonly string _appDir;

        public Updater(ILog log, DateTime currentBuild, string appDir)
        {
            _log = log;
            _currentBuild = new Version(currentBuild.Year, currentBuild.Month, currentBuild.Year);
            _appDir = appDir;
        }

        private static string? Download(string url)
        {
            using (var client = new HttpClient())
            {
                using (HttpResponseMessage? response = client.GetAsync(url).GetAwaiter().GetResult())
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string? content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        return content;
                    }
                }
            }
            return null;
        }

        private Release[] GetReleases()
        {
            string? json = Download(UpdateUrl);
            if (json == null)
            {
                _log.Warning("JSON Result download failed");
                return Array.Empty<Release>();
            }
            Release[]? result = JsonSerializer.Deserialize<Release[]>(json);
            if (result == null)
            {
                _log.Warning("JSON Result parse failed");
                return Array.Empty<Release>();
            }
            return result;
        }

        public Release? GetLatestRelease(bool preview = false)
        {
            return GetReleases()
                .OrderByDescending(x => Version.Parse(x.Version))
                .FirstOrDefault(x => x.IsPreview == preview);
        }

        public Task<Release?> GetLatestReleaseAsync(CancellationToken token, bool preview = false)
        {
            return Task.Run(() => GetLatestRelease(preview), token);
        }

        public bool IsUpdateNewerThanCurrentVersion(Release release)
        {
            return Version.Parse(release.Version) > _currentBuild;
        }

        public void LaunchUpdateScript(Release release)
        {
            string? updater = ResourceHandler.GetResourceFile<KnownFile>("Powershell/updater.ps1");

            var script = new FsPath(_appDir, "updater.ps1");
            if (script.WriteFile(_log, updater))
            {
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -File \"{script}\" \"{release.ZipPackageUrl}\" \"{release.HashSha256}\"";
                    process.Start();
                }
            }
        }
    }
}