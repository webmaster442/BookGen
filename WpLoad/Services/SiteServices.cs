//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WpLoad.Services
{
    internal class SiteServices
    {
        private static readonly string Profiles 
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                nameof(WpLoad));

        internal static void WriteDefault(string newTempFile)
        {
            var defaultConfig = new SiteInfo
            {
                Host = "Host name. lilke: https://example.com",
                Username = "Authentication user name",
                Password = "Auth password",
            };
            XmlSerializer xs = new(typeof(SiteInfo));

            using (var writer = new StringWriter())
            {
                writer.WriteLine("<!--");
                writer.WriteLine("Edit profile & save it. Close editor, when you are done");
                writer.WriteLine("-->");
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true }))
                {
                    xs.Serialize(xmlWriter, defaultConfig);
                }
                File.WriteAllText(newTempFile, writer.ToString());
            }
        }

        internal static string CreateRandomName()
        {
            Random r = new();
            StringBuilder sb = new();
            for (int i=0; i < 10; i++)
            {
                sb.Append(r.Next('0', '9'));
            }
            sb.Append(".txt");
            var temp = Path.GetTempPath();
            return Path.Combine(Path.GetTempPath(), sb.ToString());
        }

        internal static void EncryptAndDeleteTemp(string sourceTempFile, string profileName)
        {
            if (!Directory.Exists(Profiles))
            {
                Directory.CreateDirectory(Profiles);
            }
            string targetFile = Path.Combine(Profiles, profileName);
            File.Copy(sourceTempFile, targetFile);
            File.Encrypt(targetFile);
            File.Delete(sourceTempFile);
        }

        internal static bool TryReadSiteInfo(string profileName, [NotNullWhen(true)] out SiteInfo? siteInfo)
        {
            siteInfo = null;
            var sourceFile = Path.Combine(Profiles, profileName);
            if (!File.Exists(sourceFile))
            {
                return false;
            }
            File.Decrypt(sourceFile);
            using (var reader = File.OpenRead(sourceFile))
            {
                XmlSerializer xs = new XmlSerializer(typeof(SiteInfo));
                if (xs.Deserialize(reader) is SiteInfo info)
                {
                    siteInfo = info;
                }
            }
            File.Encrypt(sourceFile);
            return siteInfo != null;
        }

        internal static string[] ListProfiles()
        {
            if (!Directory.Exists(Profiles))
                return Array.Empty<string>();

            var files = Directory.GetFiles(Profiles);
            return files
                .Select(x => Path.GetFileNameWithoutExtension(x))
                .ToArray();
        }

        internal static bool TryRemove(string name)
        {
            string fullName = Path.Combine(Profiles, name);
            if (File.Exists(fullName))
            {
                File.Delete(fullName);
                return true;
            }
            return false;
        }
    }
}
