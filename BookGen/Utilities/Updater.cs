﻿//-----------------------------------------------------------------------------
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

namespace BookGen.Utilities
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

        private Release[] GetReleases()
        {
            using (var client = new WebClient())
            {
                client.UseDefaultCredentials = true;
                client.Proxy = WebRequest.GetSystemWebProxy();
                var json = client.DownloadString(new Uri(UpdateUrl));
                var result = JsonSerializer.Deserialize<Release[]>(json);
                if (result == null)
                    throw new InvalidOperationException("Error while deserializing update info...");
                
                return result;
            }
        }

        public Release? GetLatestRelease(bool preview = false)
        {
            return GetReleases()
                .OrderByDescending(x => Version.Parse(x.Version))
                .FirstOrDefault(x => x.IsPreview == preview);
        }

        public bool IsUpdateNewerThanCurrentVersion(Release release)
        {
            return Version.Parse(release.Version) > _currentBuild;
        }

        public void LaunchUpdateScript(Release release)
        {
            var updater = ResourceHandler.GetResourceFile<KnownFile>("Powershell/completer.ps1");

            FsPath script = new FsPath(_appDir, "updater.ps1");
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