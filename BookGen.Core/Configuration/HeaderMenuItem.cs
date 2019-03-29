//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Configuration
{
    public class HeaderMenuItem : MenuItem
    {
        public List<MenuItem> SubItems { get; set; }

        public bool HasChilds
        {
            get { return SubItems?.Count > 0; }
        }

        public new static HeaderMenuItem Default
        {
            get
            {
                return new HeaderMenuItem
                {
                    Title = "Menu Item title",
                    Link = "Menu Link",
                    FontAwesomeIcon = null,
                    SubItems = new List<MenuItem> { MenuItem.Default }
                };
            }
        }
    }
}
