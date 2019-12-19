//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Github;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json;

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

        public static bool GetGithubReleases(string endpoint, out List<Release> releases)
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
                releases = new List<Release>();
                return false;
            }
        }
    }
}
