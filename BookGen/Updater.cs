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

namespace BookGen
{
    internal class Updater
    {
        private const string Endpoint = "https://api.github.com/repos/webmaster442/BookGen/releases";

        private bool GetReleases(out List<Release> releases)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string response = client.DownloadString(Endpoint);
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

        private DateTime GetLinkerDate()
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var bytes = new byte[2048];

            Assembly? current = Assembly.GetAssembly(typeof(Updater));

            if (current == null)
                return DateTime.Now;

            using (var file = new FileStream(current.Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                file.Read(bytes, 0, bytes.Length);
            }

            int headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return date.AddSeconds(secondsSince1970);
        }
    }
}
