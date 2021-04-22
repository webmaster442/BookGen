//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Shell;
using BookGen.Utilities;

namespace BookGen.Framework
{
    internal abstract class ModuleBase
    {
        public abstract string ModuleCommand { get; }
        public abstract bool Execute(string[] arguments);
        public virtual string GetHelp()
        {
            return HelpUtils.GetHelpForModule(GetType().Name);
        }

        public virtual AutoCompleteItem AutoCompleteInfo
        {
            get => new AutoCompleteItem(ModuleCommand);
        }
    }
}
