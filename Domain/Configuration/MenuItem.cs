//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string FontAwesomeIcon { get; set; }

        public static MenuItem Default
        {
            get
            {
                return new MenuItem
                {
                    Title = "Menu Item title",
                    Link = "Menu Link",
                    FontAwesomeIcon = null,
                };
            }
        }
    }
}
