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

            Release? newer = UpdateUtils.SelectLatestRelease(releases, includePrerelease);

            if (newer != null)
                WriteReleaseInfo(newer);
            else
                Console.WriteLine("No releasess found");

        }

        public void UpdateProgram(bool includePrerelease, List<Release>? releaseOverride = null)
        {
            List<Release>? releases = releaseOverride;

            if (releases == null && !UpdateUtils.GetGithubReleases(Endpoint, _log, out releases))
            {
                Console.WriteLine("Error downloading releases information. Probably no Internet acces?");
                return;
            }

            Release? latestRelease = UpdateUtils.SelectLatestRelease(releases, includePrerelease);

            if (latestRelease == null)
            {
                Console.WriteLine("No releasess found");
                return;
            }

            WriteReleaseInfo(latestRelease);

            Asset? assetToDownload = UpdateUtils.SelectAssetToDownload(latestRelease);

            if (assetToDownload == null)
            {
                Console.WriteLine("Release contains no files to download");
                return;
            }


        }
    }
}
