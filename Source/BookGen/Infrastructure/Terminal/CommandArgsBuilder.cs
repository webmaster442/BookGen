using BookGen.Commands;

namespace BookGen.Infrastructure.Terminal;

internal sealed class CommandArgsBuilder
{
    private readonly List<string> _args;

    public CommandArgsBuilder()
    {
        _args = new List<string>();
    }

    public CommandArgsBuilder New()
    {
        _args.Clear();
        return this;
    }

    public CommandArgsBuilder Add(BookGenArgumentBase argumentBase)
    {
        if (argumentBase.Verbose)
        {
            _args.Add("-v");
        }

        _args.Add("-d");
        _args.Add(argumentBase.Directory);

        return this;
    }

    public CommandArgsBuilder Add(string[] strings)
    {
        _args.AddRange(strings);
        return this;
    }

    public IReadOnlyList<string> Build()
        => _args;
}
