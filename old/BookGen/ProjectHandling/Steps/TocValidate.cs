//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling.Steps;

internal sealed class TocValidate : LoadStep
{
    public TocValidate(LoadState state, ILogger log) : base(state, log)
    {
    }

    public override bool Execute()
    {
        if (State.Toc == null)
            return false;

        Log.LogInformation("Found {chapters} chapters and {files} files", State.Toc.ChapterCount, State.Toc.FilesCount);
        var tocValidator = new TocValidator(State.Toc, State.WorkDir);
        tocValidator.Validate();

        if (!tocValidator.IsValid)
        {
            Log.LogCritical("Errors found in TOC file");
            foreach (string? error in tocValidator.Errors)
            {
                Log.LogWarning("Validation error: {error}", error);
            }
            return false;
        }

        Log.LogInformation("TOC file doesn't contain any errors");
        return true;
    }
}
