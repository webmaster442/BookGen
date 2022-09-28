//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;
using WpLoad.Domain;

namespace WpLoad.Services
{
    internal static class SiteServices
    {
        private static readonly string Profiles
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                nameof(WpLoad));

        private const int step = 100;

        internal static void WriteDefault(string profileName)
        {
            if (!Directory.Exists(Profiles))
                Directory.CreateDirectory(Profiles);

            string? sourceFile = Path.Combine(Profiles, profileName + ".xml");

            var defaultConfig = new SiteInfo
            {
                ApiEndPoint = "API endpoint like https://localhost/wordpress/wp-json/wp/v2/",
                Username = "Authentication user name",
                Password = "Auth password",
            };
            XmlSerializer xs = new(typeof(SiteInfo));

            using (StreamWriter? f = File.CreateText(sourceFile))
            {
                using (var xmlWriter = XmlWriter.Create(f, new XmlWriterSettings { Indent = true }))
                {
                    xs.Serialize(xmlWriter, defaultConfig);
                }
                f.Flush();
            }
        }

        internal static bool TryReadSiteInfo(string profileName, [NotNullWhen(true)] out SiteInfo? siteInfo)
        {
            siteInfo = null;
            string? sourceFile = Path.Combine(Profiles, profileName + ".xml");
            if (!File.Exists(sourceFile))
            {
                return false;
            }
            using (FileStream? reader = File.OpenRead(sourceFile))
            {
                var xs = new XmlSerializer(typeof(SiteInfo));
                if (xs.Deserialize(reader) is SiteInfo info)
                {
                    siteInfo = info;
                }
            }
            return siteInfo != null;
        }

        internal static string[] ListProfiles()
        {
            if (!Directory.Exists(Profiles))
                return Array.Empty<string>();

            string[]? files = Directory.GetFiles(Profiles);
            return files
                .Select(x => Path.GetFileNameWithoutExtension(x))
                .ToArray();
        }

        internal static bool TryRemove(string name)
        {
            string fullName = Path.Combine(Profiles, name + ".xml");
            if (File.Exists(fullName))
            {
                File.Delete(fullName);
                return true;
            }
            return false;
        }

        internal static async Task<int> OpenEditorAndWaitClose(string profileName)
        {
            string? sourceFile = Path.Combine(Profiles, profileName + ".xml");

            using (var process = new Process())
            {
                process.StartInfo.FileName = "notepad.exe";
                process.StartInfo.Arguments = sourceFile;
                process.StartInfo.UseShellExecute = true;
                process.Start();

                int time = 0;
                while (!process.HasExited)
                {
                    time += step;
                    await Task.Delay(time);
                }
            }

            return step;
        }
    }
}
