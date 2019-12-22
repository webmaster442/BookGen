//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Domain.Github;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen
{
    internal class Updater
    {
        private const string Endpoint = "https://api.github.com/repos/webmaster442/BookGen/releases";
        private readonly ILog _log;

        public Updater(ILog log)
        {
            _log = log;
        }

        private Release? SelectLatestRelease(IEnumerable<Release> releases, bool prerelease)
        {
            return (from release in releases
                    where 
                        release.PublishDate > UpdateUtils.GetAssemblyLinkerDate()
                        && release.IsPreRelase == prerelease
                        && !release.IsDraft
                    orderby release.PublishDate descending
                    select release).FirstOrDefault();
        }

        private void WriteReleaseInfo(Release release)
        {
            Console.WriteLine("Latest release: ");
            const string prerelase = "(Pre Release)";
            Console.WriteLine("{0}: {1} {2}", release.PublishDate, release.Name, release.IsPreRelase ? prerelase : string.Empty);
            Console.WriteLine(release.Body);
        }

        public void FindNewerRelease(bool includePrerelease)
        {
            if (!UpdateUtils.GetGithubReleases(Endpoint, _log, out List<Release> releases))
            {
                Console.WriteLine("Error downloading releases information. Probably no Internet acces?");
                return;
            }

            Release? newer = SelectLatestRelease(releases, includePrerelease);

            if (newer != null)
                WriteReleaseInfo(newer);
            else
                Console.WriteLine("No releasess found");

        }

        Asset? SelectAssetToDownload(Release release)
        {
            const string zipMime = "application/x-zip-compressed";

            return (from asset in release.Assets
                    where
                        asset.ContentType == zipMime
                    select
                        asset).FirstOrDefault();
        }

        public void UpdateProgram(bool includePrerelease)
        {
            if (!UpdateUtils.GetGithubReleases(Endpoint, _log, out List<Release> releases))
            {
                Console.WriteLine("Error downloading releases information. Probably no Internet acces?");
                return;
            }

            Release? latestRelease = SelectLatestRelease(releases, includePrerelease);

            if (latestRelease == null)
            {
                Console.WriteLine("No releasess found");
                return;
            }

            WriteReleaseInfo(latestRelease);

            Asset? assetToDownload = SelectAssetToDownload(latestRelease);

            if (assetToDownload == null)
            {
                Console.WriteLine("Release contains no files to download");
                return;
            }


        }
    }
}
