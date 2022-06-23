//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.Domain.ArgumentParsing
{
    public sealed class ExternalLinksArguments : BookGenArgumentBase
    {
        [Switch("o", "output", true)]
        public FsPath OutputFile { get; set; }

        public ExternalLinksArguments()
        {
            OutputFile = FsPath.Empty;
        }
    }
}