using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Interfaces;
using System.IO;

namespace BookGen.CommandArguments
{
    internal sealed class AssemblyDocumentArguments : ArgumentsBase
    {
        [Switch("a", "assembly")]
        public FsPath AssemblyPath { get; set; }

        [Switch("o", "output")]
        public FsPath OutputDirectory { get; set; }

        [Switch("s", "singlepage")]
        public bool SinglePage { get; set; }

        public AssemblyDocumentArguments()
        {
            AssemblyPath = FsPath.Empty;
            OutputDirectory = FsPath.Empty;
        }

        public override ValidationResult Validate()
        {
            ValidationResult result = new();
            if (!AssemblyPath.IsExisting)
                result.AddIssue("assembly doesn't exist");

            if (!new FsPath(Path.ChangeExtension(AssemblyPath.ToString(), "xml")).IsExisting)
                result.AddIssue("assemlby documentation xml doesn't exist");

            if (FsPath.IsEmptyPath(OutputDirectory))
                result.AddIssue("Output directory can't be empty string");

            return result;
        }
    }
}
