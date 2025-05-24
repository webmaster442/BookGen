//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Domain.IO.Legacy;

public sealed class Asset
{
    public string Source
    {
        get;
        set;
    }

    public string Target
    {
        get;
        set;
    }

    public bool Minify
    {
        get;
        set;
    }

    public Asset()
    {
        Source = string.Empty;
        Target = string.Empty;
        Minify = false;
    }
}
