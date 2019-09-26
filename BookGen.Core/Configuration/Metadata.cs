//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public sealed class Metadata
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

        public static Metadata CreateDefault()
        {
            return new Metadata
            {
                Author = "Place author name here",
                CoverImage = "Place cover here",
                Title = "Book title"
            };
        }
    }
}
