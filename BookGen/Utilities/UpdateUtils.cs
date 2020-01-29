//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain.Github;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
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

        public static bool GetAssemblyLinkerDate(out DateTime date)
        {
            date = new DateTime();

            Assembly? current = Assembly.GetAssembly(typeof(UpdateUtils));
            if (current == null)
            {
                return false;
            }

            var attribute = current.GetCustomAttribute<AssemblyBuildDateAttribute>();

            if (attribute != null)
            {
                date = attribute.BuildDate.Date;
                return true;
            }

            return false;
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
            if (!GetAssemblyLinkerDate(out DateTime date))
            {
                return null;
            }

            return (from release in releases
                    where
                        release.PublishDate > date
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

        public static string? GetProgramDir(string? additionalDir = null)
        {
            string? progdir = AppDomain.CurrentDomain.BaseDirectory;
            if (additionalDir == null)
                return progdir;
            else if (progdir != null)
                return Path.Combine(progdir, additionalDir);
            else
                return null;
        }

        public async static Task<bool> DowloadAsssetAsyc(Asset toDownload, string targetFile, ILog log, IProgress<double> progress)
        {
            if (string.IsNullOrEmpty(toDownload.DownloadUrl))
                return false;

            double lastreport = -1;

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

                                double percent = downloaded / toDownload.Size;

                                if (Math.Abs(lastreport - percent) > 0.01)
                                {
                                    percent = Math.Round(percent, 3);
                                    progress.Report(percent);
                                    lastreport = percent;
                                }
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

        internal static void ExecuteReplaceScript(string programDir, bool terminateCaller = true)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = programDir;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Arguments = $"/c \"{Path.Combine(programDir, "ReplaceContents.bat")}\"";
            process.Start();

            if (terminateCaller)
            {
                var current = System.Diagnostics.Process.GetCurrentProcess();
                current.Kill();
            }
        }

        public static bool CreateReplaceScript(string targetPath, string ProgramName, string tempfile, ILog log)
        {
            Assembly? current = Assembly.GetAssembly(typeof(UpdateUtils));

            if (current == null)
                return false;

            Stream? resource = current.GetManifestResourceStream("BookGen.Resources.ReplaceContents.bat");

            if (resource == null)
                return false;

            try
            {
                using (var reader = new StreamReader(resource))
                {
                    var text = reader.ReadToEnd().Trim();
                    text = text.Replace("{{program}}", ProgramName);
                    text = text.Replace("{{tempfile}}", tempfile);
                    var file = Path.Combine(targetPath, "ReplaceContents.bat");
                    using (StreamWriter target = File.CreateText(file))
                    {
                        target.Write(text);
                        return true;
                    }
                }
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException
                                    || ex is PathTooLongException
                                    || ex is IOException)
            {
                log.Warning(ex);
                return false;
            }
        }

        public static bool ExtractZip(string source, string target, ILog log)
        {
            try
            {
                ZipFile.ExtractToDirectory(source, target, true);
                return true;
            }
            catch (Exception ex) when (ex is PathTooLongException
                                    || ex is IOException
                                    || ex is FileNotFoundException
                                    || ex is UnauthorizedAccessException
                                    || ex is InvalidDataException)
            {
                log.Warning(ex);
                return false;
            }
        }
    }
}
