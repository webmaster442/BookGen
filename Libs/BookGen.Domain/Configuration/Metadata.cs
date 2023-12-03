//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces.Configuration;

namespace BookGen.Domain.Configuration
{
    public sealed class Metadata : IReadOnlyMetadata
    {
        [Doc("Author name")]
        public string Author
        {
            get;
            set;
        }

        [Doc("Cover image for social sites, etc..")]
        public string CoverImage
        {
            get;
            set;
        }

        [Doc("Title")]
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
}
