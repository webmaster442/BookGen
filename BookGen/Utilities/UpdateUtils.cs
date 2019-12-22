//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Domain.Github;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;

namespace BookGen.Utilities
{
    internal static class UpdateUtils
    {
        private static WebClient CreateClient()
        {
            var client = new WebClient();
            IWebProxy proxy = WebRequest.DefaultWebProxy;
            proxy.Credentials = CredentialCache.DefaultCredentials;
            client.Proxy = proxy;
            client.Headers.Add(HttpRequestHeader.UserAgent, "BookGen Autoupdater");
            return client;
        }

        public static DateTime GetAssemblyLinkerDate()
        {
            Assembly? current = Assembly.GetAssembly(typeof(UpdateUtils));

            if (current == null)
                return DateTime.Now;

            Stream? resource = current.GetManifestResourceStream("BookGen.Resources.BuildDate.txt");

            if (resource == null)
                return DateTime.Now;

            using (var reader = new StreamReader(resource))
            {
                var text = reader.ReadToEnd().Trim();

                if (DateTime.TryParse(text, out DateTime time))
                {
                    return time;
                }

                return DateTime.Now;
            }
        }

        public static bool GetGithubReleases(string endpoint, ILog log, out List<Release> releases)
        {
            try
            {
                using (var client = CreateClient())
                {
                    string response = client.DownloadString(endpoint);
                    releases = JsonSerializer.Deserialize<List<Release>>(response);
                    return true;
                }
            }
            catch (Exception ex) when (ex is WebException || ex is JsonException)
            {
                log.Warning(ex);
                releases = new List<Release>();
                return false;
            }
        }

        public static Release? SelectLatestRelease(IEnumerable<Release> releases, bool prerelease)
        {
            return (from release in releases
                    where
                        release.PublishDate > UpdateUtils.GetAssemblyLinkerDate()
                        && release.IsPreRelase == prerelease
                        && !release.IsDraft
                        && release.Assets != null
                        && release.Assets.Count > 0
                    orderby release.PublishDate descending
                    select release).FirstOrDefault();
        }

        public static Asset? SelectAssetToDownload(Release release)
        {
            const string zipMime = "application/x-zip-compressed";

            return (from asset in release.Assets
                    where
                        asset.ContentType == zipMime
                    select
                        asset).FirstOrDefault();
        }

        public async static Task<bool> DowloadFileAsyc(Asset toDownload, string targetFile, ILog log, IProgress<double> progress)
        {
            if (string.IsNullOrEmpty(toDownload.DownloadUrl))
                return false;

            try
            {
                byte[] buffer = new byte[4096];
                using (var client = CreateClient())
                {
                    using (var stream = await client.OpenReadTaskAsync(toDownload.DownloadUrl).ConfigureAwait(false))
                    {
                        using (var target = File.Create(targetFile))
                        {
                            double downloaded = 0;
                            int recieved = 0;
                            do
                            {
                                recieved = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                                downloaded += recieved;
                                target.Write(buffer, 0, recieved);
                                progress.Report(downloaded / toDownload.Size);
                            }
                            while (recieved > 0);
                        }
                    }
                }
                return true;
            }
            catch (WebException ex)
            {
                log.Warning(ex);
                return false;
            }
        }
    }
}
