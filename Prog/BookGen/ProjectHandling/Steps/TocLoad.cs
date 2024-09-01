//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling.Steps;

internal sealed class TocLoad : LoadStep
{
    public TocLoad(LoadState state, ILogger log) : base(state, log)
    {
    }

    public override bool Execute()
    {
        if (State.Config == null)
            return false;

        Log.LogInformation("Parsing TOC file...");
        FsPath? tocFile = State.WorkDir.Combine(State.Config.TOCFile);

        if (!tocFile.IsExisting)
        {
            Log.LogCritical("Table of contents doesn't exist: {tocFile}", tocFile);
            return false;
        }

        State.Toc = MarkdownUtils.ParseToc(tocFile.ReadFile(Log));

        return State.Toc.FilesCount > 0;
    }
}
