//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    public class Metadata
    {
        public string Author { get; set; }
        public string CoverImage { get; set; }
        public string Title { get; set; }

        public static Metadata Default
        {
            get
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
}
