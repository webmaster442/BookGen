//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Shell;

namespace BookGen.Framework
{
    internal abstract class ModuleBase
    {
        public abstract string ModuleCommand { get; }
        public abstract bool Execute(string[] arguments);
        public abstract string GetHelp();
        public virtual AutoCompleteItem AutoCompleteInfo
        {
            get => new AutoCompleteItem(ModuleCommand);
        }
    }
}
