using BookGen.Interfaces;

namespace BookGen.CommandArguments
{
    internal sealed class ExternalLinksArguments : BookGenArgumentBase
    {
        [Switch("o", "output", true)]
        public FsPath OutputFile { get; set; }

        public ExternalLinksArguments()
        {
            OutputFile = FsPath.Empty;
        }
    }
}
