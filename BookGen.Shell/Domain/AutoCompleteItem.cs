using System.Collections.Generic;
using System.Linq;

namespace BookGen.Shell.Domain
{
    public class AutoCompleteItem
    {
        public string ModuleName { get;}
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
