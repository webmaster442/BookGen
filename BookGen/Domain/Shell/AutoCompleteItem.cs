//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace BookGen.Domain.Shell
{
    internal class AutoCompleteItem
    {
        public string ModuleName { get; }
        public List<string> ArgumentList { get; }

        public AutoCompleteItem(string name)
        {
            ModuleName = name;
            ArgumentList = new List<string>();
        }

        public AutoCompleteItem(string name, params string[] arguments)
        {
            ModuleName = name;
            ArgumentList = arguments.ToList();
        }
    }
}
