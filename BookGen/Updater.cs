//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Domain.Github;
using BookGen.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BookGen
{
    internal class Updater: IProgress<double>
    {
        private const string Endpoint = "https://api.github.com/repos/webmaster442/BookGen/releases";
        private readonly ILog _log;

        private readonly string _programdir;
        private readonly string _newprogramdir;
        private readonly bool _canKillCaller;

        public Updater(ILog log, string? progdir = null, string? downloaddir = null)
        {
            _log = log;
            _canKillCaller = false;

            string? programDir;
            string? newProgramDir;

            programDir = progdir ?? UpdateUtils.GetProgramDir();
            newProgramDir = downloaddir ?? UpdateUtils.GetProgramDir("new");;

            if (programDir == UpdateUtils.GetProgramDir())
                _canKillCaller = true;

            if (programDir == null || newProgramDir == null)
                throw new InvalidOperationException();

            _programdir = programDir;
            _newprogramdir = newProgramDir;
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
                _log.Warning("Error downloading releases information. Probably no Internet acces?");
                return;
            }

            Release? newer = UpdateUtils.SelectLatestRelease(releases, includePrerelease);

            if (newer != null)
                WriteReleaseInfo(newer);
            else
                Console.WriteLine("No releasess found");

        }

        public void Report(double value)
        {
            const int barWidth = 70;
            StringBuilder bar = new StringBuilder(barWidth);
            double limit = Math.Floor(value * barWidth);
            for (int i=0; i<barWidth; i++)
            {
                if (i < limit)
                    bar.Append('=');
                else
                    bar.Append(' ');
            }
            Console.WriteLine("[{0}] {1}%\r", bar, value * 100);
        }

        public async Task<bool> UpdateProgram(bool includePrerelease, string ProgramName, List<Release>? releaseOverride = null)
        {
            List<Release>? releases = releaseOverride;

            if (_programdir == null || _newprogramdir == null)
            {
                _log.Warning("Failed to find program directory");
                return false;
            }

            if (releases == null && !UpdateUtils.GetGithubReleases(Endpoint, _log, out releases))
            {
                _log.Warning("Error downloading releases information. Probably no Internet acces?");
                return false;
            }

            Release? latestRelease = UpdateUtils.SelectLatestRelease(releases, includePrerelease);

            if (latestRelease == null)
            {
                _log.Warning("No releasess found");
                return false;
            }

            WriteReleaseInfo(latestRelease);

            Asset? assetToDownload = UpdateUtils.SelectAssetToDownload(latestRelease);

            if (assetToDownload == null)
            {
                _log.Warning("Release contains no files to download");
                return false;
            }

            var tempfile = Path.GetTempFileName();

            bool result = await UpdateUtils.DowloadAsssetAsyc(assetToDownload, tempfile, _log, this); 

            if (!result)
            {
                _log.Warning("Download error");
                return false;
            }

            if (!UpdateUtils.ExtractZip(tempfile, _newprogramdir, _log))
            {
                _log.Warning("Downloaded zip file corrupt");
                return false;
            }

            if (!UpdateUtils.CreateReplaceScript(_programdir, ProgramName, tempfile, _log))
            {
                _log.Warning("Replace file generation error");
                return false;
            }

            UpdateUtils.ExecuteReplaceScript(_programdir, _canKillCaller);

            return true;
        }
    }
}
