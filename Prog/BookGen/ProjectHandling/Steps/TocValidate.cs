//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling.Steps;

internal sealed class TocValidate : LoadStep
{
    public TocValidate(LoadState state, ILog log) : base(state, log)
    {
    }

    public override bool Execute()
    {
        if (State.Toc == null)
            return false;

        Log.Info("Found {0} chapters and {1} files", State.Toc.ChapterCount, State.Toc.FilesCount);
        var tocValidator = new TocValidator(State.Toc, State.WorkDir);
        tocValidator.Validate();

        if (!tocValidator.IsValid)
        {
            Log.Critical("Errors found in TOC file: ");
            foreach (string? error in tocValidator.Errors)
            {
                Log.Warning(error);
            }
            return false;
        }

        Log.Info("TOC file doesn't contain any errors");
        return true;

    }
}
