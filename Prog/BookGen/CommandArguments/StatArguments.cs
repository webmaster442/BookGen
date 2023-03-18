namespace BookGen.CommandArguments;

internal sealed class StatArguments : ArgumentsBase
{
    [Switch("d", "dir")]
    public string Directory { get; set; }

    [Switch("i", "input")]
    public string Input { get; set; }

    public StatArguments()
    {
        Directory = Environment.CurrentDirectory;
        Input = string.Empty;
    }
}
