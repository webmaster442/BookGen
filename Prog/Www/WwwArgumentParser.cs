namespace Www;
internal class WwwArgumentParser
{
    private readonly string[] _args;

    public WwwArgumentParser(string[] args)
    {
        _args = args;
    }

    public bool HasArguments
        => _args.Length > 0;

    public bool FirstParamIsUrl
        => HasArguments && Uri.TryCreate(_args[0], UriKind.RelativeOrAbsolute, out _);

    public bool IsBangFormat
        => _args.Length >= 2;
}
