//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Gui.ArgumentParser;
using System.IO;

namespace BookGen.Domain.ArgumentParsing
{
    internal sealed class AssemblyDocumentArguments : ArgumentsBase
    {
        [Switch("a", "assembly", true)]
        public FsPath AssemblyPath { get; set; }

        [Switch("o", "output", true)]
        public FsPath OutputDirectory { get; set; }

        [Switch("s", "singlepage")]
        public bool SinglePage { get; set; }

        public AssemblyDocumentArguments()
        {
            AssemblyPath = FsPath.Empty;
            OutputDirectory = FsPath.Empty;
        }

        public override bool Validate()
        {
            return
                AssemblyPath.IsExisting
                && new FsPath(Path.ChangeExtension(AssemblyPath.ToString(), "xml")).IsExisting
                && !FsPath.IsEmptyPath(OutputDirectory);
        }
    }
}
