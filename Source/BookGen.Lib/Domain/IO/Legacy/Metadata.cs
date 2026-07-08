//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Lib.Domain.IO.Legacy;

public sealed class Metadata
{
    public string Author
    {
        get;
        set;
    }

    public string CoverImage
    {
        get;
        set;
    }

    public string Title
    {
        get;
        set;
    }

    public Metadata()
    {
        Author = "Place author name here";
        CoverImage = "Place cover here";
        Title = "Book title";
    }
}
