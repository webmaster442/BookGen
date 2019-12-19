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
using System.Threading.Tasks;

namespace BookGen.Utilities
{
    internal static class UpdateUtils
    {
        private static DateTime? _assemblyLinkTime = null;

        public static DateTime GetAssemblyLinkerDate()
        {
            if (_assemblyLinkTime.HasValue)
                return _assemblyLinkTime.Value;

            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var bytes = new byte[2048];

            Assembly? current = Assembly.GetAssembly(typeof(UpdateUtils));

            if (current == null)
                return DateTime.Now;

            using (var file = new FileStream(current.Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                file.Read(bytes, 0, bytes.Length);
            }

            int headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _assemblyLinkTime = date.AddSeconds(secondsSince1970);

            return _assemblyLinkTime.Value;
        }

        public static bool GetGithubReleases(string endpoint, ILog log, out List<Release> releases)
        {
            try
            {
                using (var client = new WebClient())
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

        public async static Task<bool> DowloadFileAsyc(Asset toDownload, string targetFile, ILog log, IProgress<double> progress)
        {
            if (string.IsNullOrEmpty(toDownload.DownloadUrl))
                return false;

            try
            {
                byte[] buffer = new byte[4096];
                using (var client = new WebClient())
                {
                    using (var stream = await client.OpenReadTaskAsync(toDownload.DownloadUrl))
                    {
                        using (var target = File.Create(targetFile))
                        {
                            double downloaded = 0;
                            int recieved = 0;
                            do
                            {
                                recieved = await stream.ReadAsync(buffer, 0, buffer.Length);
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
