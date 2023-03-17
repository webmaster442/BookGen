using BookGen.Domain.ArgumentParsing;

namespace BookGen.CommandArguments
{
    public sealed class BuildArguments : BookGenArgumentBase
    {
        [Switch("a", "action", true)]
        public BuildAction? Action { get; set; }

        [Switch("n", "nowait")]
        public bool NoWaitForExit { get; set; }
    }
}
