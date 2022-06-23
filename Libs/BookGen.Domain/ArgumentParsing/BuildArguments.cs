//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


namespace BookGen.Domain.ArgumentParsing
{
    public sealed class BuildArguments : BookGenArgumentBase
    {
        [Switch("a", "action", true)]
        public BuildAction? Action { get; set; }

        [Switch("n", "nowait")]
        public bool NoWaitForExit { get; set; }
    }
}
