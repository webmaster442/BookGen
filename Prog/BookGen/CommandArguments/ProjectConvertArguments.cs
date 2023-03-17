using BookGen.Cli;

namespace BookGen.CommandArguments
{
    internal sealed class ProjectConvertArguments : ArgumentsBase
    {
        [Switch("b", "backup")]
        public bool Backup { get; set; }

        [Switch("d", "dir")]
        public string Directory { get; set; }

        public ProjectConvertArguments()
        {
            Directory = Environment.CurrentDirectory;
        }
    }
}
