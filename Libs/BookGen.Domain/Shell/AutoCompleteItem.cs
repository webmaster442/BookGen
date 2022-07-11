//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain.Shell
{
    public class AutoCompleteItem
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
