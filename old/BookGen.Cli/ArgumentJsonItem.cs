namespace BookGen.Cli;

public sealed class ArgumentJsonItem
{
    public required string Name { get; init; }
    public required string[] Arguments { get; init; }
}
