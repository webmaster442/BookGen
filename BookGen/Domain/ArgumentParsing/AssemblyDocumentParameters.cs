//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;

namespace BookGen.Domain.ArgumentParsing
{
    internal class AssemblyDocumentParameters
    {
        public FsPath AssemblyPath { get; set; }
        public FsPath XmlPath { get; set; }
        public FsPath OutputDirectory { get; set; }

        public AssemblyDocumentParameters()
        {
            AssemblyPath = FsPath.Empty;
            XmlPath = FsPath.Empty;
            OutputDirectory = FsPath.Empty;
        }
    }
}
