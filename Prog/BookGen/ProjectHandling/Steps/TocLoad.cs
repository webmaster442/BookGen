using BookGen.Interfaces;

namespace BookGen.ProjectHandling.Steps
{
    internal sealed class TocLoad : LoadStep
    {
        public TocLoad(LoadState state, ILog log) : base(state, log)
        {
        }

        public override bool Execute()
        {
            if (State.Config == null)
                return false;

            Log.Info("Parsing TOC file...");
            FsPath? tocFile = State.WorkDir.Combine(State.Config.TOCFile);

            if (!tocFile.IsExisting)
            {
                Log.Critical("Table of contents doesn't exist: {0}", tocFile);
                return false;
            }

            State.Toc = MarkdownUtils.ParseToc(tocFile.ReadFile(Log));

            return State.Toc != null;
        }
    }
}
