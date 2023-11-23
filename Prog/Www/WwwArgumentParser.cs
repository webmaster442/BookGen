//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Www;

internal sealed class WwwArgumentParser
{
    private readonly string[] _args;

    public WwwArgumentParser(string[] args)
    {
        _args = args;
    }

    public bool IsHelpRequested
        => _args.Any(x => x == "-h" || x == "--help");

    public bool HasArguments
        => _args.Length > 0;

    public bool FirstParamIsUrl
        => HasArguments && Uri.TryCreate(_args[0], UriKind.RelativeOrAbsolute, out _);

    public bool IsBangFormat
        => _args.Length >= 2;
}
