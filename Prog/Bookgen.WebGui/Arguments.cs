using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.WebGui;

internal sealed class Arguments : ArgumentsBase
{
    [Switch("d", "dir")]
    public string Directory { get; set; }

    public Arguments()
    {
        Directory = Environment.CurrentDirectory;
    }
}
