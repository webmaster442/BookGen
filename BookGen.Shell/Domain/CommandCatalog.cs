//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Shell.Domain
{
    public static class CommandCatalog
    {
        public static IEnumerable<AutoCompleteItem> Commands
        {
            get
            {



                yield return new AutoCompleteItem("Help");

                yield return new AutoCompleteItem("SubCommands");
            }
        }
    }
}
