using BookGen.Update.Dto;
using BookGen.Update.Infrastructure;

namespace BookGen.Update.Steps;

internal class CheckIfUpdateNeeded : IUpdateStepSync
{
    private static Release? GetLatestRelease(Release[] releases, bool preview = false)
    {
        return releases
            .OrderByDescending(x => Version.Parse(x.Version))
            .FirstOrDefault(x => x.IsPreview == preview);
    }

    public bool Execute(GlobalState state)
    {
        return true;
    }
}
