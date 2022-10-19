using BookGen.Update.Dto;
using BookGen.Update.Infrastructure;
using System.Reflection;

namespace BookGen.Update.Steps;

internal sealed class CheckIfUpdateNeeded : IUpdateStepSync
{
    private static Release? GetLatestRelease(Release[] releases, bool preview = false)
    {
        return releases
            .OrderByDescending(x => Version.Parse(x.Version))
            .FirstOrDefault(x => x.IsPreview == preview);
    }

    private static Version GetCurrentVersion()
    {
        return Assembly
            .GetAssembly(typeof(CheckIfUpdateNeeded))
            ?.GetName()
            .Version ?? new Version(0, 0);
    }

    public bool Execute(GlobalState state)
    {
        Release? latestRelease = GetLatestRelease(state.Releases);
        if (latestRelease == null)
        {
            state.Issues.Add("Couldn't find latest release");
            return false;
        }

        var latest = Version.Parse(latestRelease.Version);
        Version current = GetCurrentVersion();
        if (latest < current)
        {
            state.Issues.Add("Current version is {current} is newer than latest ({latest}) release");
            state.Issues.Add("No update needed");
            return false;
        }

        state.Latest = latestRelease;
        return true;
    }
}
