//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    public sealed class MetaTag
    {
        public string Author { get; set; }
        public string SiteName { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public MetaTag()
        {
            Author = string.Empty;
            SiteName = string.Empty;
            ImageUrl = string.Empty;
            Title = string.Empty;
            Url = string.Empty;
            Description = string.Empty;
        }
    }
}
